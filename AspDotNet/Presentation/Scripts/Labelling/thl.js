(function () {
    'use strict'

    var menuId, pageName;
    var toolbarId, pageId, $divTblEl, $divDetailsEl, $toolbarEl, $tblMasterEl, $formEl;
    var orderStatus;
    var tableParams = {
        offset: 0,
        limit: 10,
        sort: '',
        order: '',
        filter: ''
    };

    $(function () {
        if (!menuId)
            menuId = localStorage.getItem("menuId");
        if (!pageName)
            pageName = localStorage.getItem("pageName");

        pageId = pageName + "-" + menuId;
        $divTblEl = $(globalConstants.DIV_TBL_ID_PREFIX + pageId);
        toolbarId = globalConstants.TOOLBAR_ID_PREFIX + pageId;
        $toolbarEl = $(toolbarId);
        $tblMasterEl = $(globalConstants.MASTER_TBL_ID_PREFIX + pageId);
        $formEl = $(globalConstants.FORM_ID_PREFIX + pageId);
        $divDetailsEl = $(globalConstants.DIV_DETAILS_ID_PREFIX + pageId);

        initMasterTbl();
        orderStatus = orderStatusConstants.PENDING;
        getMasterTblData();

        $formEl.find("#btnDownloadTemplate").on('click', function (e) {
            e.preventDefault();
            window.open('/api/labelling/download-template?labelType=' + pageName, '_blank');
        });

        $formEl.find("#tclFile").change(function (e) {
            e.preventDefault();
            var l = $(this).ladda();
            l.ladda('start');

            $formEl.find(':checkbox').each(function () {
                this.value = this.checked;
            });

            const config = {
                headers: {
                    'content-type': 'multipart/form-data',
                    'Authorization': "Bearer " + localStorage.getItem("token")
                }
            }
            var formData = new FormData();
            var data = $formEl.serializeArray();

            $.each(data, function (i, v) {
                formData.append(v.name, v.value);
            });

            formData.append("tclFile", $formEl.find("#tclFile")[0].files[0]);
            var selectedOrderFor = $formEl.find("#OrderForId").select2('data');
            if (selectedOrderFor)
                formData.append("OrderFor", selectedOrderFor[0].text);

            axios.post("/api/labelling/uk-and-ce/process-file", formData, config)
                .then(function (response) {
                    l.ladda('stop');
                    $("#uploadPanel-" + pageId).find(".alert").alert('close');
                    initChildTbl(response.data);
                })
                .catch(function (error) {
                    l.ladda('stop');
                    if (error.response.status == 400)
                        showUploadError(error.response.data.Message);
                    else
                        toastr.error(error.response.data.Message);
                });
        });

        $toolbarEl.find("#btnNew").on('click', function (e) {
            e.preventDefault();
            toggleActiveToolbarBtn(this, $toolbarEl);
            resetForm();
            $divTblEl.fadeOut();
            $divDetailsEl.fadeIn();
            initOrderFor();
        });

        $toolbarEl.find("#btnPending").on("click", function (e) {
            e.preventDefault();
            toggleActiveToolbarBtn(this, $toolbarEl);
            resetTableParams();
            orderStatus = orderStatusConstants.PENDING;
            getMasterTblData();
        });

        $toolbarEl.find("#btnAcknowledge").on("click", function (e) {
            e.preventDefault();
            toggleActiveToolbarBtn(this, $toolbarEl);
            resetTableParams();
            orderStatus = orderStatusConstants.ACKNOWLEDGE;
            getMasterTblData();
        });

        $toolbarEl.find("#btnReject").on("click", function (e) {
            e.preventDefault();
            toggleActiveToolbarBtn(this, $toolbarEl);
            resetTableParams();
            orderStatus = orderStatusConstants.REJECT;
            getMasterTblData();
        });

        $toolbarEl.find("#btnPrinting").on("click", function (e) {
            e.preventDefault();
            toggleActiveToolbarBtn(this, $toolbarEl);
            resetTableParams();
            orderStatus = orderStatusConstants.PRINTING;
            getMasterTblData();
        });

        $toolbarEl.find("#btnReadyforDelivery").on("click", function (e) {
            e.preventDefault();
            toggleActiveToolbarBtn(this, $toolbarEl);
            resetTableParams();
            orderStatus = orderStatusConstants.READY_FOR_DELIVERY;
            getMasterTblData();
        });

        $toolbarEl.find("#btnShipped").on("click", function (e) {
            e.preventDefault();
            toggleActiveToolbarBtn(this, $toolbarEl);
            resetTableParams();
            orderStatus = orderStatusConstants.SHIPPED;
            getMasterTblData();
        });

        $toolbarEl.find("#btnDelivered").on("click", function (e) {
            e.preventDefault();
            toggleActiveToolbarBtn(this, $toolbarEl);
            resetTableParams();
            orderStatus = orderStatusConstants.DELIVERYED;
            getMasterTblData();
        });

        $toolbarEl.find("#btnAll").on("click", function (e) {
            e.preventDefault();
            toggleActiveToolbarBtn(this, $toolbarEl);
            resetTableParams();
            orderStatus = orderStatusConstants.ALL;
            getMasterTblData();
        });

        $formEl.find("#btnCancel").on("click", function (e) {
            e.preventDefault();
            backToList();
        });

        $formEl.find("#btnClear").on("click", function (e) {
            e.preventDefault();
            resetForm();
            $("#uploadPanel-" + pageId).find(".alert").alert('close');
        });

        $formEl.find("#btnSave").click(function (e) {
            e.preventDefault();
            var l = $(this).ladda();
            l.ladda('start');

            $formEl.find(':checkbox').each(function () {
                this.value = this.checked;
            });

            const config = {
                headers: {
                    'content-type': 'multipart/form-data',
                    'Authorization': "Bearer " + localStorage.getItem("token")
                }
            }
            var formData = new FormData();
            var data = $formEl.serializeArray();

            $.each(data, function (i, v) {
                formData.append(v.name, v.value);
            });

            formData.append("tclFile", $formEl.find("#tclFile")[0].files[0]);
            var selectedOrderFor = $formEl.find("#OrderForId").select2('data');
            if (selectedOrderFor)
                formData.append("OrderFor", selectedOrderFor[0].text);

            axios.post("/api/labelling/uk-and-ce", formData, config)
                .then(function (response) {
                    l.ladda('stop');
                    toastr.success("TSL Upload completed successfully.");
                    backToList();
                })
                .catch(function (error) {
                    l.ladda('stop');
                    if (error.response.status == 400)
                        showUploadError(error.response.data.Message);
                    else
                        toastr.error(error.response.data.Message);
                });
        });
    });

    function initMasterTbl() {
        $tblMasterEl.bootstrapTable('destroy');
        $tblMasterEl.bootstrapTable({
            showRefresh: true,
            showExport: true,
            showColumns: true,
            toolbar: toolbarId,
            exportTypes: "['csv', 'excel']",
            pagination: true,
            filterControl: true,
            searchOnEnterKey: true,
            sidePagination: "server",
            pageList: "[10, 25, 50, 100, 500]",
            cache: false,
            showFooter: true,
            columns: [
                {
                    title: 'Actions',
                    align: 'center',
                    width: 100,
                    formatter: function (value, row, index, field) {
                        return getMasterTblRowActions(row);
                    },
                    events: {
                        'click .edit': function (e, value, row, index) {
                            e.preventDefault();
                            getDetailsInfo(row.Id);
                        },
                        'click .approve': function (e, value, row, index) {
                            e.preventDefault();

                            showBootboxConfirm("Approve Yarn PO", "Are you sure you want to approve this PO?", function (yes) {
                                if (yes) {
                                    var url = "/scdapi/approve-ypo/" + row.Id;
                                    axios.post(url)
                                        .then(function () {
                                            toastr.success(constants.APPROVE_SUCCESSFULLY);
                                            getMasterTblData();
                                        })
                                        .catch(function (error) {
                                            toastr.error(error.response.data.Message);
                                        });
                                }
                            });
                        },
                        'click .reject': function (e, value, row, index) {
                            e.preventDefault();

                            showBootboxPrompt("Reject Yarn PO", "Are you sure you want to reject this PO?", function (result) {
                                if (result) {
                                    var data = {
                                        Id: row.Id,
                                        UnapproveReason: result
                                    };

                                    axios.post("/scdapi/reject-ypo", data)
                                        .then(function () {
                                            toastr.success(constants.REJECT_SUCCESSFULLY);
                                            getMasterTblData();
                                        })
                                        .catch(function (error) {
                                            toastr.error(error.response.data.Message);
                                        });
                                }
                            });
                        }
                    }
                },
                {
                    field: "OrderNo",
                    title: "Order No",
                    filterControl: "input",
                    width: 100,
                    filterControlPlaceholder: globalConstants.FILTER_CONTROL_PLACEHOLDER
                },
                {
                    field: "OrderDateStr",
                    title: "Order Date",
                    filterControl: "input",
                    width: 80,
                    filterControlPlaceholder: globalConstants.FILTER_CONTROL_PLACEHOLDER
                },
                {
                    field: "CustomerName",
                    title: "Customer Name",
                    filterControl: "input",
                    width: 180,
                    filterControlPlaceholder: globalConstants.FILTER_CONTROL_PLACEHOLDER
                },
                {
                    field: "OrderFor",
                    title: "Order For",
                    filterControl: "input",
                    width: 80,
                    filterControlPlaceholder: globalConstants.FILTER_CONTROL_PLACEHOLDER
                },
                {
                    field: "OrderStatus",
                    title: "Status",
                    width: 80
                },
                {
                    field: "RevisionCount",
                    title: "Revision",
                    width: 80
                }
            ],
            onPageChange: function (number, size) {
                var newOffset = (number - 1) * size;
                var newLimit = size;
                if (tableParams.offset == newOffset && tableParams.limit == newLimit)
                    return;

                tableParams.offset = newOffset;
                tableParams.limit = newLimit;

                getMasterTblData();
            },
            onSort: function (name, order) {
                tableParams.sort = name;
                tableParams.order = order;
                tableParams.offset = 0;

                getMasterTblData();
            },
            onRefresh: function () {
                resetTableParams();
                getMasterTblData();
            },
            onColumnSearch: function (columnName, filterValue) {
                if (columnName in filterBy && !filterValue) {
                    delete filterBy[columnName];
                }
                else
                    filterBy[columnName] = filterValue;

                if (Object.keys(filterBy).length === 0 && filterBy.constructor === Object)
                    tableParams.filter = "";
                else
                    tableParams.filter = JSON.stringify(filterBy);

                getMasterTblData();
            }
        });
    }

    function initChildTbl(data) {
        $("#tblChild").bootstrapTable('destroy');
        $("#tblChild").bootstrapTable({
            showExport: true,
            showColumns: true,
            exportTypes: "['csv', 'excel']",
            pagination: true,
            sidePagination: "client",
            pageList: "[10, 25, 50, 100, 500]",
            columns: [
                {
                    field: "PackType",
                    title: "Pack Type",
                    filterControl: "input",
                    width: 100
                },
                {
                    field: "Season",
                    title: "Season",
                    filterControl: "input",
                    width: 100
                },
                {
                    field: "TPND",
                    title: "TPND",
                    filterControl: "input",
                    width: 100
                },
                {
                    field: "PONo",
                    title: "PO No",
                    filterControl: "input",
                    width: 100
                },
                {
                    field: "UKStyleRef",
                    title: "Uk Style Ref",
                    filterControl: "input",
                    width: 100
                },
                {
                    field: "CEStyleRef",
                    title: "CE Style Ref",
                    filterControl: "input",
                    width: 100
                },
                {
                    field: "ShortDesc",
                    title: "Short Desc",
                    filterControl: "input",
                    width: 100
                },
                {
                    field: "EQOSCode",
                    title: "Eqos Code",
                    filterControl: "input",
                    width: 100
                },
                {
                    field: "BarcodeNo",
                    title: "Barcode No",
                    filterControl: "input",
                    width: 100
                },
                {
                    field: "Supplier",
                    title: "Supplier",
                    filterControl: "input",
                    width: 100
                },
                {
                    field: "PackagingSupplier",
                    title: "Packaging Supplier",
                    filterControl: "input",
                    width: 100
                },
                {
                    field: "Dept",
                    title: "Dept",
                    filterControl: "input",
                    width: 100
                },
                {
                    field: "Brand",
                    title: "Brand",
                    filterControl: "input",
                    width: 100
                },
                {
                    field: "Qty",
                    title: "Qty",
                    filterControl: "input",
                    width: 100
                }
            ],
            data: data
        });
    }

    function getMasterTblData() {
        var queryParams = $.param(tableParams);
        $tblMasterEl.bootstrapTable('showLoading');
        var url = "/api/labelling/uk-and-ce" + "?labellingType=" + pageName + "&orderStatus=" + orderStatus + "&" + queryParams;
        axios.get(url)
            .then(function (response) {
                $tblMasterEl.bootstrapTable('load', response.data);
                $tblMasterEl.bootstrapTable('hideLoading');
            })
            .catch(function (err) {
                toastr.error(err.response.data.Message);
            })
    }

    function getDetailsInfo(id) {
        axios.get("/api/labelling/uk-and-ce-details/" + id)
            .then(function (response) {
                var data = response.data;
                resetForm();
                setFormData($formEl, data);
                $formEl.find("#OrderNo").show();
                $divTblEl.fadeOut();
                $divDetailsEl.fadeIn();

                $formEl.find("#btnDownloadAttachedFile").remove();
                $formEl.find("#uploadButtonGroup").append('<button class="btn btn-primary" type="button" id="btnDownloadAttachedFile"><i class="fa fa-download"></i> Download Attached File</button>');
                $formEl.find("#btnDownloadAttachedFile").on("click", function (e) {
                    e.preventDefault();
                    axios.head(data.FilePath)
                        .then(function () {
                            var downloadUrl = '/api/labelling/download-attached?path=' + data.FilePath;
                            window.open(downloadUrl, '_blank');
                        })
                        .catch(function () {
                            toastr.error("File doesn't exists!");
                        });
                });

                initChildTbl(data.Childs);
            })
            .catch(function (err) {
                console.error(err);
                toastr.error(globalConstants.LOAD_ERROR_MESSAGE);
            });
    }

    function resetForm() {
        $formEl.trigger("reset");
        $.each($formEl.find('select'), function (i, el) {
            $(el).select2('');
        });
        $formEl.find("#Id").val(-1111);
        $formEl.find("#EntityState").val(4);
    }

    function resetTableParams() {
        tableParams.offset = 0;
        tableParams.limit = 10;
        tableParams.filter = '';
        tableParams.sort = '';
        tableParams.order = '';
    }

    function backToList() {
        $divDetailsEl.fadeOut();
        resetForm();
        $divTblEl.fadeIn();

        getMasterTblData();
    }

    function initOrderFor() {
        axios.get("/api/select-option/ukandcelabellingtype?type=" + pageName)
            .then(function (response) {
                initSelect2($formEl.find("#OrderForId"), response.data);
                $formEl.find("#OrderForId").on('select2:select select2:unselect', function (evt) {
                    if (evt.params._type === "unselect")
                        $formEl.find("#tclFile").attr("disabled", "disabled");
                    else
                        $formEl.find("#tclFile").removeAttr("disabled");
                })
            })
            .catch(function (err) {
                console.error(err);
            })
    }

    function showUploadError(errorMessage) {
        var content = '<div class="alert alert-warning alert-dismissible fade in" role="alert">'
            + '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">×</span></button>'
            + '<p>' + errorMessage + '</p>'
            + '</div>';
        $("#uploadPanel-" + pageId).find(".alert").alert('close');
        $("#uploadPanel-" + pageId).prepend(content);
    }

    function getMasterTblRowActions(row) {
        var rowActions = [];

        switch (orderStatus) {
            case orderStatusConstants.PENDING:
                rowActions = ['<span class="btn-group">',
                    '<a class="btn btn-xs btn-primary edit" href="javascript:void(0)" title="Edit PO">',
                    '<i class="fa fa-edit" aria-hidden="true"></i>',
                    '</a>',
                    '<a class="btn btn-xs btn-primary" href="/reports/InlinePdfView?ReportId=3&HasExternalReport=true&ExternalId=' + row.OrderForId + '&OrderNo=' + row.OrderNo + '" target="_blank" title="PO Report">',
                    '<i class="fa fa-file-pdf-o" aria-hidden="true"></i>',
                    '</a>',
                    '</span>'];
                break;
            case orderStatusConstants.ACKNOWLEDGE:
                break;
            case orderStatusConstants.REJECT:
                break;
            case orderStatusConstants.PRINTING:
                break;
            case orderStatusConstants.READY_FOR_DELIVERY:
                break;
            case orderStatusConstants.SHIPPED:
                break;
            case orderStatusConstants.DELIVERYED:
                break;
            case orderStatusConstants.ALL:
                break;
            default:
        }

        return rowActions.join(' ');
    }
})();