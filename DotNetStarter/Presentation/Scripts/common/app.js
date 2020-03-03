// Please follow these rules to update this page
// 
// Thanks

'use strict'

var rootPath;
var HasAutoNumber = false;
var CanInsert = false;
var CanUpdate = false;
var CanDelete = false;
var ImageFileLength, PICINID, SupplierID, BuyerIds;
var currentTab;
var template = '';
var activeMenu = false;
var $mainTab, $mainTabContent;

$(document).ready(function () {
    rootPath = window.location.protocol + '//' + window.location.host;

    toastr.options.escapeHtml = true;
    $.fn.editable.defaults.mode = 'inline';
    axios.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded';
    if (localStorage.getItem("token")) {
        axios.defaults.headers.common['Authorization'] = "Bearer " + localStorage.getItem("token");
    }
    //getBookingAnalysisBookingAcknowledgementCountALL();
    loadProgressBar();

    GetMenus();

    $('[data-toggle="tooltip"]').tooltip({ html: true });

    $mainTab = $("#mainTab");
    $mainTabContent = $("#mainTabContent");
    //when ever any tab is clicked this method will be call
    $mainTab.on("click", "a", function (e) {
        e.preventDefault();

        $(this).tab('show');
        currentTab = $(this);

        var menuLink = e.target.hash.split('#')[1];
        var el = $("[data-action-name='" + menuLink + "']");
        $('.treeview li').removeClass('active');
        el.parent().addClass("active");
    });

    registerCloseEvent();

    setTimeout(refreshToken, 1500000);
});

function refreshToken() {
    var params = new URLSearchParams();
    params.append('grant_type', 'refresh_token');
    params.append('client_id', localStorage.getItem("client_id"));
    params.append('refresh_token', localStorage.getItem("refresh_token"));

    axios.post("/token", params)
        .then(function (response) {
            localStorage.setItem("token", response.data.access_token);
            axios.defaults.headers.common['Authorization'] = "Bearer " + response.data.access_token;
        })
        .catch(function (err) {
            toastr.error("Error occured while updating bearer token.");
            console.error(err);
        });

    setTimeout(refreshToken, 1500000);
}

function logout() {
    axios.post('/api/Account/Logout')
        .then(function () {
            window.location.href = "/account/logoff";
        })
        .catch(function () {
            window.location.href = "/account/logoff";
        });
}

// #region Table Formatter
function priceFormatter(value, row, index) {
    var p = value.toFixed(2).split(".");
    var formatedPrice = p[0].split("").reduceRight(function (acc, value, i, orig) {
        var pos = orig.length - i - 1;
        return value + (pos && !(pos % 3) ? "," : "") + acc;
    }, "") + (p[1] ? "." + p[1] : "");

    if (row.CurrencyCode === "USD")
        formatedPrice = "$" + formatedPrice;
    return formatedPrice;
}

function responseHandler(res) {
    return JSON.parse(res);
}
// #endregion

// #region Table button click events
var actionEventsPO = {
    'click .view-po': function (e, value, row, inded) {
        var poNo = row.SPONo ? row.SPONo : row.PONo;
        viewPO(poNo, row.SubGroupName, row.IsSwo);
    }
}

function refreshTable(tableId, url) {
    $(tableId).bootstrapTable('refresh', { "url": url });
}
function reloadTableData(type, tableId) {
    $.get("/PI/GetSPOMasterForNewPI?supplierId=" + SupplierID + "&editType=" + type, function (response) {
        $(tableId).bootstrapTable('load', JSON.parse(response));
    });
}
// #endregion

// #region Tree Plugin Initialization
/* Tree()
 * ======
 * Converts a nested list into a multilevel
 * tree view menu.
 *
 * @Usage: $('.my-menu').tree(options)
 *         or add [data-widget="tree"] to the ul element
 *         Pass any option as data-option="value"
 */
+function ($) {
    'use strict';

    var DataKey = 'lte.tree';

    var Default = {
        animationSpeed: 500,
        accordion: true,
        followLink: false,
        trigger: '.treeview a'
    };

    var Selector = {
        tree: '.tree',
        treeview: '.treeview',
        treeviewMenu: '.treeview-menu',
        open: '.menu-open, .active',
        li: 'li',
        data: '[data-widget="tree"]',
        active: '.active'
    };

    var ClassName = {
        open: 'menu-open',
        tree: 'tree'
    };

    var Event = {
        collapsed: 'collapsed.tree',
        expanded: 'expanded.tree'
    };

    // Tree Class Definition
    // =====================
    var Tree = function (element, options) {
        this.element = element;
        this.options = options;

        $(this.element).addClass(ClassName.tree);

        $(Selector.treeview + Selector.active, this.element).addClass(ClassName.open);

        this._setUpListeners();
    };

    Tree.prototype.toggle = function (link, event) {
        var treeviewMenu = link.next(Selector.treeviewMenu);
        var parentLi = link.parent();
        var isOpen = parentLi.hasClass(ClassName.open);

        if (!parentLi.is(Selector.treeview)) {
            return;
        }

        if (!this.options.followLink || link.attr('href') === '#') {
            event.preventDefault();
        }

        if (isOpen) {
            this.collapse(treeviewMenu, parentLi);
        } else {
            this.expand(treeviewMenu, parentLi);
        }
    };

    Tree.prototype.expand = function (tree, parent) {
        var expandedEvent = $.Event(Event.expanded);

        if (this.options.accordion) {
            var openMenuLi = parent.siblings(Selector.open);
            var openTree = openMenuLi.children(Selector.treeviewMenu);
            this.collapse(openTree, openMenuLi);
        }

        parent.addClass(ClassName.open);
        tree.slideDown(this.options.animationSpeed, function () {
            $(this.element).trigger(expandedEvent);
        }.bind(this));
    };

    Tree.prototype.collapse = function (tree, parentLi) {
        var collapsedEvent = $.Event(Event.collapsed);

        //tree.find(Selector.open).removeClass(ClassName.open);
        parentLi.removeClass(ClassName.open);
        tree.slideUp(this.options.animationSpeed, function () {
            //tree.find(Selector.open + ' > ' + Selector.treeview).slideUp();
            $(this.element).trigger(collapsedEvent);
        }.bind(this));
    };

    // Private

    Tree.prototype._setUpListeners = function () {
        var that = this;

        $(this.element).on('click', this.options.trigger, function (event) {
            that.toggle($(this), event);

            if ($(this).parent().hasClass("treeview"))
                return;

            $('.treeview li').removeClass('active');
            $(this).parent().addClass("active");
            $('.treeview').removeClass('menu-open');
            $(this).closest('li.treeview').addClass("menu-open");

            var actionName = $(this).data("action-name");
            var pageType = $(this).data("page-type");
            var pageName = $(this).data("page-name");
            if (pageName)
                pageName = pageName.split(' ').join('');
            else
                pageName = actionName;

            var menuId = $(this).data("menu-id");
            var tabCaption = $(this).text().trim();
            var isExists = $('#mainTab a[href="#' + pageName + '"]');

            if (pageType == 'Report')
                return;
            else if (pageType == 'NF') {
                if (isExists.length === 0) {
                    $mainTab.append('<li><a href="#' + pageName + '">' + tabCaption + '<span class="close closeTab fa fa-times" type="button"><i class="icon-remove"></i></span></a></li>');
                    GetNotFoundViewMarkup(pageName);
                }
                else {
                    showTab(pageName);
                }
            }
            else if (pageType == 'CI') {
                if (isExists.length === 0) {
                    $mainTab.append('<li><a href="#' + pageName + '">' + tabCaption + '<span class="close closeTab fa fa-times" type="button"><i class="icon-remove"></i></span></a></li>');
                    getCommonInterfaceMarkup(menuId, pageName);
                }
                else {
                    showTab(pageName);
                }
            }
            else {
                if (isExists.length === 0) {
                    $mainTab.append('<li><a href="#' + pageName + '">' + tabCaption + '<span class="close closeTab fa fa-times" type="button"><i class="icon-remove"></i></span></a></li>');

                    var controllerName = $(this).data("controller-name");
                    GetViewMarkup(controllerName, actionName, menuId, pageName);
                }
                else {
                    showTab(pageName);
                    var tableId = $(this).data("table-id");
                    var url = $(this).data("refresh-url");
                    if (tableId && url)
                        refreshTable(tableId, url);
                }
            }
        });
    };

    // Plugin Definition
    // =================
    function Plugin(option) {
        return this.each(function () {
            var $this = $(this);
            var data = $this.data(DataKey);

            if (!data) {
                var options = $.extend({}, Default, $this.data(), typeof option === 'object' && option);
                $this.data(DataKey, new Tree($this, options));
            }
        });
    }

    var old = $.fn.tree;

    $.fn.tree = Plugin;
    $.fn.tree.Constructor = Tree;

    // No Conflict Mode
    // ================
    $.fn.tree.noConflict = function () {
        $.fn.tree = old;
        return this;
    };

    //// Tree Data API
    //// =============
    //$(window).on('load', function () {
    //    $(Selector.data).each(function () {
    //        Plugin.call($(this));
    //    });
    //});

}(jQuery);
// #endregion 

// #region Implelmentation of Tab View
//shows the tab with passed content div id..paramter tabid indicates the div where the content resides
function showTab(tabId) {
    $('#mainTab a[href="#' + tabId + '"]').tab('show');
}
//return current active tab
function getCurrentTab() {
    return currentTab;
}

//This function will create a new tab here and it will load the url content in tab content div.
function craeteNewTabAndLoadUrl(parms, url, loadDivSelector) {

    $("" + loadDivSelector).load(url, function (response, status, xhr) {
        if (status === "error") {
            var msg = "Sorry but there was an error getting details ! ";
            $("" + loadDivSelector).html(msg + xhr.status + " " + xhr.statusText);
            $("" + loadDivSelector).html("Load Ajax Content Here...");
        }
    });

}

function getElement(selector) {
    var tabContentId = $currentTab.attr("href");
    return $("" + tabContentId).find("" + selector);

}

function removeCurrentTab() {
    var tabContentId = $currentTab.attr("href");
    $currentTab.parent().remove(); //remove li of tab
    $('#mainTab a:last').tab('show'); // Select first tab
    $(tabContentId).remove(); //remove respective tab content
}

function registerCloseEvent() {
    $(".closeTab").click(function () {
        var tabContentId = $(this).parent().attr("href");
        $(this).parent().parent().remove(); //remove li of tab
        $('#mainTab a:last').tab('show'); // Select first tab
        $(tabContentId).remove(); //remove respective tab content
    });
}

function GetViewMarkup(controller, actionName, menuId, pageName) {
    var url = "/" + controller + "/" + actionName + "?menuId=" + menuId + "&pageName=" + pageName;
    $.get(url, function (htmlResponse) {
        var markup = '<div class="tab-pane" id="' + pageName + '">' + htmlResponse + '</div>';
        $mainTabContent.append(markup);
        showTab(pageName);
        registerCloseEvent();

        var scriptPath = '/Scripts/' + controller + '/' + actionName + '.js?menuId=' + menuId + '&version=' + $("#versionNumer").val();
        if ($('script[src="' + scriptPath + '"]').length === 0) {
            var s = document.createElement('script');
            s.setAttribute('src', scriptPath);
            $("#" + pageName).append(s);
        }
    });
}

function clickAccountNavigation(event) {
    var target = event.currentTarget;
    var actionName = target.dataset.actionName;
    var isExists = $('#mainTab a[href="#' + actionName + '"]');
    if (isExists.length === 0) {
        var tabCaption = target.innerText.trim();
        $mainTab.append('<li><a href="#' + actionName + '">' + tabCaption + '<span class="close closeTab fa fa-times" type="button"><i class="icon-remove"></i></span></a></li>');

        var controllerName = target.dataset.controllerName;
        getAccountViewMarkup(controllerName, actionName);
    }
    else {
        showTab(actionName);
    }
}

function getAccountViewMarkup(controller, actionName) {
    $.get("/" + controller + "/" + actionName, function (htmlResponse) {
        var markup = '<div class="tab-pane" id="' + actionName + '">' + htmlResponse + '</div>';
        $mainTabContent.append(markup);
        showTab(actionName);
        registerCloseEvent();

        var scriptPath = '/Scripts/' + controller + '/' + actionName + '.js' + "?version=" + $("#versionNumer").val();
        if ($('script[src="' + scriptPath + '"]').length === 0) {
            var s = document.createElement('script');
            s.setAttribute('src', scriptPath);
            $("#" + actionName).append(s);
        }
    });
}

function GetNotFoundViewMarkup(tabid) {
    $.get("/home/notfoundpartial", function (htmlResponse) {
        var markup = '<div class="tab-pane" id="' + tabid + '">' + htmlResponse + '</div>';
        $mainTabContent.append(markup);
        showTab(tabid);
        registerCloseEvent();
    });
}

function getCommonInterfaceMarkup(menuId, pageName) {
    localStorage.setItem("current_common_interface_menuid", menuId);
    var url = "/admin/commoninterface?menuId=" + menuId;
    $.get(url, function (htmlResponse) {
        var markup = '<div class="tab-pane" id="' + pageName + '">' + htmlResponse + '</div>';

        $mainTabContent.append(markup);
        showTab(pageName);
        registerCloseEvent();

        var scriptPath = '/Scripts/common/common-interface.js' + "?version=" + menuId;
        if ($('script[src="' + scriptPath + '"]').length === 0) {
            var s = document.createElement('script');
            s.setAttribute('src', scriptPath);
            $("#" + pageName).append(s);
        }
    })
}

function GetMenus() {
    axios.get("/api/getmenus/" + globalConstants.APPLICATION_ID)
        .then(function (response) {
            generateMenu(response.data);
            $(".sidebar-menu").append(template);
            $(".sidebar-menu").tree();
        })
        .catch(function (err) {
            console.log(err.response.data);
        })
}

function generateMenu(menuList) {
    $.each(menuList, function (i, item) {
        if (!item.Childs.length) {
            var navProperties = item.NavigateUrl.split('/');
            if (navProperties[1] == 'notfoundpartial') {
                template += '<li><a href="#!" data-controller-name="' + navProperties[0] + '" data-action-name="' + item.PageName + '" data-page-name="' + item.PageName + '" data-menu-id="' + item.MenuId + '" data-page-type="NF"><i class="fa fa-circle-o"></i> <span>' + item.MenuCaption + '</span></a></li>';
            }
            else if (item.UseCommonInterface) {
                template += '<li><a href="#! data-controller-name="' + navProperties[0] + '" data-action-name="' + navProperties[1] + '" data-page-name="' + item.PageName + '" data-menu-id="' + item.MenuId + '" data-page-type="CI"><i class="fa fa-circle-o"></i> <span>' + item.MenuCaption + '</span></a></li>';
            }
            else if (item.PageName == 'ReportViewer') {
                var path = rootPath + '/reports/index';
                template += '<li><a href="' + path + '" target="_blank" data-page-type="Report"><i class="fa fa-circle-o"></i> <span>' + item.MenuCaption + '</span></a></li>';
            }
            else {
                template += '<li><a href="#!" data-controller-name="' + navProperties[0] + '" data-action-name="' + navProperties[1] + '" data-table-id="' + navProperties[2] + '" data-page-name="' + item.PageName + '" data-menu-id="' + item.MenuId + '"><i class="fa fa-circle-o"></i> <span>' + item.MenuCaption + '</span></a></li>';
            }

            activeMenu = false;
            return true;
        }
        else {
            activeMenu = item.MenuId == 509 ? true : false;
            var active = activeMenu ? "active" : "";
            template += '<li class="treeview ' + active + '">';
            template += '<a href="#">'
                + '<i class="fa fa-circle"></i><span>' + item.MenuCaption + '</span>'
                + '<span class="pull-right-container">'
                + '<i class="fa fa-angle-left pull-right"></i>'
                + '</span>'
                + '</a>';
            template += '<ul class="treeview-menu">';
            activeMenu = false;
        }

        generateMenu(item.Childs);

        template += '</ul></li>';
    });
}

// #region Constants
var constants = Object.freeze({
    LOAD_ERROR_MESSAGE: "An error occured in fetching your data",
    SUCCESS_MESSAGE: "Your record saved successfully!",
    PROPOSE_SUCCESSFULLY: "Your record has been sent for approval!",
    APPROVE_SUCCESSFULLY: "Your record approved successfylly!",
    ACCEPTED_SUCCESFULLY: "Your record accepted successfully!",
    REJECT_SUCCESSFULLY: "Your record rejected successfylly!",
    UNAPPROVE_SUCCESSFULLY: "Your record unapproved successfully",
    REVISE_BOOKING: "Your booking is under Revised Stage!!!"
});

var globalConstants = Object.freeze({
    APPLICATION_ID: 1,
    PAGE_ID_PREFIX: "#",
    DIV_TBL_ID_PREFIX: "#divtbl",
    TOOLBAR_ID_PREFIX: "#toolbar",
    MASTER_TBL_ID_PREFIX: "#tbl",
    FORM_ID_PREFIX: "#form",
    DIV_DETAILS_ID_PREFIX: "#divDetails",
    FILTER_CONTROL_PLACEHOLDER: "Type & Enter for Search",
    LOAD_ERROR_MESSAGE: "Error loading data."
});

var orderStatusConstants = Object.freeze({
    PENDING: 1,
    ACKNOWLEDGE: 2,
    REJECT: 3,
    PRINTING: 4,
    READY_FOR_DELIVERY: 5,
    SHIPPED: 6,
    DELIVERYED: 7,
    ALL: 8
});

var labelTypes = Object.freeze({
    TCL: "TCL",
    THL: "THL",
    TSL: "TSL"
});
// #endregion