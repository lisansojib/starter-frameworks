﻿'use strict'

var rootPath;
var HasAutoNumber = false;
var CanInsert = false;
var CanUpdate = false;
var CanDelete = false;
var template = '';
var activeMenu = false;
var reportId, hasExternalReport, buyerId;
var columnValues = [];
var columnValueOptions = [];
var shownColumnValues = [];
var params = "";
var currentRow;

$(document).ready(function () {
    rootPath = window.location.protocol + '//' + window.location.host;

    $.fn.editable.defaults.mode = 'inline';
    toastr.options.escapeHtml = true;
    axios.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded';
    if (localStorage.getItem("token")) {
        axios.defaults.headers.common['Authorization'] = "Bearer " + localStorage.getItem("token");
    }

    loadProgressBar();

    getMenus();

    $('[data-toggle="tooltip"]').tooltip({ html: true });

    $("#modal-default").modal('show');

    $("#btnOk").on('click', function (e) {
        e.preventDefault();
        var selectedFilterValue = $("#tblFilterSetValue").bootstrapTable('getSelections');
        selectedFilterValue = selectedFilterValue.map(function (el) { return el.value; });
        currentRow.ColumnValue = selectedFilterValue.toString();
        refreshRow(currentRow);
        $("#modal-filterset").modal('hide');
    });

    $("#btnClear").on("click", function (e) {
        e.preventDefault();
        clearParameters();
    });

    $("#btnPreveiw").on("click", function (e) {
        e.preventDefault();
        previewReport();
    });

    $("#btnPdf").on("click", function (e) {
        e.preventDefault();
        previewReport(constants.PDF);
    });

    refreshToken();
});

function refreshToken() {
    var formData = new FormData();
    formData.append("grant_type", "refresh_token");
    formData.append("client_id", localStorage.getItem("client_id"));
    formData.append("refresh_token", localStorage.getItem("refresh_token"));
    axios.post(url, formData)
        .then(function (response) {
            localStorage.setItem("token", response.data.access_token);
        })
        .catch(function () {
            M.toast({ html: 'Error occured while updating bearer token.' });
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

            reportId = $(this).data("report-id");
            hasExternalReport = $(this).data("has-external-report");
            if (hasExternalReport)
                showReportBuyerSelection();
            else
                loadReportInformation();
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

function getMenus() {
    axios.get("/reports/GetMenus?applicationId=" + constants.APPLICATION_ID)
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
            template += '<li><a href="#!" data-report-id="' + item.ReportId + '" data-has-external-report="' + item.HasExternalReport + '"><i class="fa fa-circle-o"></i> <span>' + item.Node_Text + '</span></a></li>';

            activeMenu = false;
            return true;
        }
        else {
            activeMenu = item.MenuId == 509 ? true : false;
            var active = activeMenu ? "active" : "";
            template += '<li class="treeview ' + active + '">';
            template += '<a href="#">'
                + '<i class="fa fa-circle"></i><span>' + item.Node_Text + '</span>'
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

function showReportBuyerSelection() {
    axios.get("/api/selectoption/report-buyer")
        .then(function (response) {
            var buyerList = convertToSelectOptions(response.data);
            showBootboxSelectPrompt("Select Buyer", buyerList, "small", function (result) {
                if (result) {
                    var currentBuyer = buyerList.find(function (x) {
                        return x.value == result;
                    });

                    buyerId = parseInt(result);

                    $("#rightNav").prepend('<li id="liBuyer"><a href="#!">Buyer&nbsp;<i class="fa fa-chevron-right"></i>&nbsp;' + currentBuyer.text + '</a></li>');
                    loadReportInformation();
                }
                console.log(result);
            });
        })
        .catch(function (err) {
            toastr.error(err);
        });
}

function loadReportInformation() {
    if (!hasExternalReport) {
        buyerId = 0;
        $("#liBuyer").remove();
    }

    var url = "/reports/GetReportInformation?reportId=" + reportId + "&hasExternalReport=" + hasExternalReport + "&buyerId=" + buyerId; 

    axios.get(url)
        .then(function (response) {
            columnValues = response.data.FilterSetList;
            columnValueOptions = response.data.ColumnValueOptions;
            shownColumnValues = columnValues.filter(function (el) {
                return !el.IsSystemParameter && !el.IsHidden && el.ColumnName != "Expression";
            });
            initTable(shownColumnValues);
        })
        .catch(function (err) {
            console.log(err.response.data);
        });
}

function initTable(data) {
    $("#tblReportFilters").bootstrapTable('destroy');
    $("#tblReportFilters").bootstrapTable({
        cache: false,
        uniqueId: 'ColumnName',
        columns: [
            {
                field: "Caption",
                title: "...",
                align: 'center',
                width: 50
            },
            {
                field: "ColumnName",
                title: "Expression",
                width: 100
            },
            {
                field: "Operators",
                title: "...",
                align: 'center',
                width: 50
            },
            {
                field: "ColumnValue",
                title: "Value",
                width: 300,
                formatter: function (value, row, index, field) {
                    var cValue = !row.ColumnValue ? "Set " + row.ColumnName : row.ColumnValue;
                    return ['<a href="javascript:void(0)" class="editable-link edit">' + cValue + '</a>'].join(' ');
                },
                events: {
                    'click .edit': function (e, value, row, index) {
                        e.preventDefault();
                        currentRow = row;

                        if (row.DataType == "DateTime" || row.DataType == "System.DateTime") {
                            $(e.target).datepicker({
                                autoclose: true,
                                todayHighlight: true,
                                endDate: "0d",
                                todayBtn: true
                            }).on('changeDate', function (e) {
                                try {
                                    //if (row.ColumnName == "FromDate") {
                                    //    if()
                                    //}
                                    row.ColumnValue = moment(e.date).format("MM/DD/YYYY");
                                    refreshRow(row);
                                } catch (e) {
                                    row.ColumnValue = "";
                                    console.log(e);
                                }
                            });
                        }
                        else if (row.DataType == "String" || row.DataType == "System.String") {
                            var selectOptions = [];
                            if (row.IsMultipleSelection) { // Multiple Value
                                var title = "Select " + row.ColumnName;

                                if (row.HasParent) { // If Parent/Dependency column is required
                                    params = "?ReportId=" + reportId + "&MethodName=" + encodeURIComponent(row.MethodName);
                                    var parentColumns = row.ParentColumn.split(',');
                                    $.each(parentColumns, function (i, column) {
                                        var cv = columnValues.find(function (el) { return el.ColumnName == column });
                                        if (!cv) {
                                            toastr.warning("Please set " + column + " value first");
                                            return false;
                                        }
                                        params += '&' + column + '=' + encodeURIComponent(cv.ColumnValue);
                                    });

                                    var url = "/Reports/GetFilterColumnOptions" + params;
                                    axios.get(url)
                                        .then(function (response) {
                                            $.each(response.data, function (i, v) {
                                                var item = v.find(function (el) { return el.Key === row.ValueColumnId });
                                                selectOptions.push({ value: item.Value, text: item.Value })
                                            });

                                            initTableFilterValue(selectOptions);
                                            $("#modal-filterset-title").text(title);
                                            $("#modal-filterset").modal('show');
                                        })
                                        .catch(function (err) {
                                            console.log(err);
                                        })
                                }
                                else { // If no Parent/Dependency column
                                    $.each(columnValueOptions, function (i, v) {
                                        var el = v.find(function (el) { return el.Key === row.ColumnName });
                                        var option = { value: el.Value, text: el.Value };
                                        selectOptions.push(option);
                                    });

                                    initTableFilterValue(selectOptions);
                                    $("#modal-filterset-title").text(title);
                                    $("#modal-filterset").modal('show');
                                }
                            }
                            else { // Single Value
                                var title = "Select " + row.ColumnName;

                                if (row.HasParent) { // If Parent/Dependency column is required
                                    params = "?ReportId=" + reportId + "&MethodName=" + row.MethodName
                                    var parentColumns = row.ParentColumn.split(',');
                                    $.each(parentColumns, function (i, column) {
                                        var cv = columnValues.find(function (el) { return el.ColumnName == column });
                                        if (!cv) {
                                            toastr.warning("Please set " + column + " value first");
                                            return false;
                                        }
                                        params += '&' + column + '=' + cv.ColumnValue;
                                    });

                                    var url = "/Reports/GetFilterColumnOptions" + params;
                                    axios.get(url)
                                        .then(function (response) {
                                            selectOptions = response.data.map(function (el) { return { value: el, text: el } });
                                            showBootboxSelectPrompt(title, selectOptions, "", function (result) {
                                                if (result) {
                                                    row.ColumnValue = result;
                                                    refreshRow(row);
                                                }
                                            });
                                        })
                                        .catch(function (err) {
                                            console.log(err);
                                        })
                                }
                                else { // If no Parent/Dependency column
                                    $.each(columnValueOptions, function (i, v) {
                                        var el = v.find(function (el) { return el.Key === row.ColumnName });
                                        var option = { value: el.Value, text: el.Value };
                                        selectOptions.push(option);
                                    });

                                    showBootboxSelectPrompt(title, selectOptions, "", function (result) {
                                        if (result) {
                                            row.ColumnValue = result;
                                            refreshRow(row);
                                        }                                        
                                    });
                                }
                            }
                        }
                    }
                }
            },
            {
                field: "OrAnd",
                title: "...",
                align: 'center',
                width: 50
            }
        ],
        data: data
    });
}

function refreshRow(data) {
    var columnValuesArray = data.ColumnValue.split(',');
    data.Operators = columnValuesArray.length > 1 ? "In" : "=";
    $("#tblReportFilters").bootstrapTable('updateByUniqueId', { id: data.ColumnName, row: data });
}

function initTableFilterValue(data) {
    $("#tblFilterSetValue").bootstrapTable('destroy');
    $("#tblFilterSetValue").bootstrapTable({
        pagination: true,
        showFooter: true,
        pageSize: 10,
        pageList: [10, 25, 50, 100],
        columns: [
            {
                align: 'center',
                checkbox: true,
                width: 50
            },
            {
                field: "value",
                title: "...",
                align: 'center',
                sortable: true
            }
        ],
        data: data
    });
}

function clearParameters() {
    $.each(columnValues, function (i, cv) {
        cv.ColumnValue = "";
        cv.Operators = "=";
    });
}

function previewReport(reportType) {
    toastr.info("Please wait while we process your report.");

    var requiredFilterSets = columnValues.filter(function (el) { return el.Caption == "***" });
    var isValid = true;
    $.each(requiredFilterSets, function (i, cv) {
        if (!cv.ColumnValue) {
            isValid = false;
            toastr.error(cv.ColumnName + " is required.");
        }            
    });

    if (!isValid) return;

    var params = $.param({ ReportId: reportId, FilterSetList: JSON.stringify(columnValues) });
    if (reportType == constants.PDF) {
        var url = rootPath + "/reports/pdfview?" + params;
        window.open(url, "_blank");
    } else {
        var url = rootPath + "/reports/reportview.aspx?" + params;
        window.open(url, "_blank");
    }
}

// #region Constants
var constants = Object.freeze({
    APPLICATION_ID : 8,
    LOAD_ERROR_MESSAGE: "An error occured in fetching your data",
    SUCCESS_MESSAGE: "Your record saved successfully!",
    PROPOSE_SUCCESSFULLY: "Your record has been sent for approval!",
    APPROVE_SUCCESSFULLY: "Your record approved successfylly!",
    ACCEPTED_SUCCESFULLY: "Your record accepted successfully!",
    REJECT_SUCCESSFULLY: "Your record rejected successfylly!",
    UNAPPROVE_SUCCESSFULLY: "Your record unapproved successfully",
    REVISE_BOOKING: "Your booking is under Revised Stage!!!",
    PDF: "PDF"
});
// #endregion