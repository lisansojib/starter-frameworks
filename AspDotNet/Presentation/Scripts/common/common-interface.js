﻿(function () {
    'use strict'

    // #region variable declarations
    var constraints = [];
    var childConstraints = [];
    var masterForm;
    var childForm;
    var interfaceConfigs;
    var menuId;
    var filterBy = {};
    var finderApiUrl = "";
    var childGridApiUrl = "";
    var draftChilds = {
        total: 0,
        rows: []
    };
    
    var selectColumn = {
        id: '',
        placeholder: '',
        apiUrl: '',
        hasDependentColumn: '',
        dependentColumnName: '',
        defaultValue: '',
        data: []
    }

    var selectColumnList = [];
    var childSelectColumnList = [];

    var finderTableParams = {
        offset: 0,
        limit: 10,
        sort: '',
        order: '',
        filter: ''
    };
    var childGridTableParams = {
        offset: 0,
        limit: 10,
        sort: '',
        order: '',
        filter: ''
    };
    // #endregion

    $(function () {
        menuId = localStorage.getItem("current_common_interface_menuid");
        masterForm = $("#form-ci-" + menuId);
        childForm = $("#form-ci-child-" + menuId);
        getInterfaceChilds();
        

        $("#btnSaveMaster-" + menuId).click(function (e) {
            e.preventDefault();
            saveMaster();
        });

        $("#btnSaveChild-" + menuId).click(function (e) {
            e.preventDefault();
            saveChild();
        });

        $("#btnNewChildItem-" + menuId).click(function (e) {
            e.preventDefault();
            resetChildForm();

            var l = $('#btnNewChildItem-' + menuId).ladda();
            l.ladda('start');

            if (!validateMasterForm()) {
                l.ladda('stop');
                return;
            }

            l.ladda('stop');

            $("#modal-child-" + menuId).modal('show');
        });
    });

    // #region Genereting markup
    function getInterfaceChilds() {
        var url = '/api/commoninterface?menuId=' + menuId;
        axios.get(url)
            .then(function (response) {
                interfaceConfigs = response.data;
                $("#title-form-ci-" + menuId).text(interfaceConfigs.InterfaceName);
                $("#finderTitle-" + menuId).text(interfaceConfigs.InterfaceName + " List")

                generateElements();
                initControls();

                if (interfaceConfigs.HasGrid) {
                    $("#title-child-grid-" + menuId).text(interfaceConfigs.ChildGrids[0].ChildGridName);
                    $("#boxChildGrid-" + menuId).show();
                    generateChildGrid();
                    generateChildElements();
                    initChildControls();
                }
            })
            .catch(function (error) {
                console.log(error)
            })
    }

    function generateElements() {
        var template = "";
        $.each(interfaceConfigs.Childs, function (i, value) {
            switch (value.EntryType) {
                case "text":
                    if (value.IsSys) {
                        template += '<div class="form-group">'
                            + '<label class="col-sm-3 control-label">' + value.Label + '</label>'
                            + '<div class="col-sm-5"><div class="input-group input-group-sm">'
                            + '<input type="text" class="form-control" id="' + value.ColumnName + '" name="' + value.ColumnName + '" readonly>'
                            + '<span class="input-group-btn">'
                            + '<button type="button" class="btn btn-default" id="ci-new-' + menuId + '"><i class="fa fa-plus" aria-hidden="true"></i></button>'
                            + '<button type="button" class="btn btn-success" id="ci-finder-' + menuId + '"><i class="fa fa-search" aria-hidden="true"></i></button>'
                            + '</span></div></div></div>';
                    }
                    else if (!value.IsSys && value.IsHidden && value.IsEnable) {
                        template += '<input type="hidden" id="' + value.ColumnName + '" name="' + value.ColumnName + '" value=""/>'
                    }
                    else {
                        template += '<div class="form-group">'
                            + '<label class="col-sm-3 control-label">' + value.Label + '</label>'
                            + '<div class="col-sm-5">'
                            + '<input type="text" class="form-control input-sm" id="' + value.ColumnName + '" name="' + value.ColumnName + '" placeholder="' + value.Label + '">'
                            + '</div></div>';
                    }
                    break;
                case "datetime":
                case "date":
                    template += '<div class="form-group">'
                        + '<label class="col-sm-3 control-label">' + value.Label + '</label>'
                        + '<div class="col-sm-5">'
                        + '<input type="text" class="form-control input-sm bootstrap-datepicker" id="' + value.ColumnName + '" name="' + value.ColumnName + '" placeholder="' + value.Label + '">'
                        + '</div></div>';
                    break;
                case "select":
                    template += '<div class="form-group">'
                        + '<label class="col-sm-3 control-label">' + value.Label + '</label>'
                        + '<div class="col-sm-5">'
                        + '<select class="form-control" id="' + value.ColumnName + '" name="' + value.ColumnName + '" style="width: 100%"></select>'
                        + '</div></div>';

                    var selectColumn = {};
                    selectColumn.id = value.ColumnName;
                    selectColumn.placeholder = value.Label;
                    selectColumn.apiUrl = value.SelectApiUrl;
                    selectColumn.hasDependentColumn = value.HasDependentColumn;
                    selectColumn.dependentColumnName = value.DependentColumnName;
                    selectColumn.defaultValue = value.DefaultValue;
                    selectColumnList.push(selectColumn);
                    break;
                case "number":
                    template += '<div class="form-group">'
                        + '<label class="col-sm-3 control-label">' + value.Label + '</label>'
                        + '<div class="col-sm-5">'
                        + '<input type="number" class="form-control input-sm" id="' + value.ColumnName + '" name="' + value.ColumnName + '" placeholder="' + value.Label + '">'
                        + '</div></div>';
                    break;
                case "checkbox":
                    template += '<div class="form-group"><div class="col-sm-offset-2 col-sm-10"><div class="checkbox checkbox-success">'
                        + '<input type="checkbox" id="' + value.ColumnName + '" name="' + value.ColumnName + '"> <label for="' + value.ColumnName + '">' + value.Label + '</label></div></div></div>';
                    break;
                default:
                    break;
            }

            if (value.IsRequired) {
                constraints[value.ColumnName] = value.MaxLength > 0 ? { presence: true, length: { maximum: value.MaxLength } } : { presence: true };
            }
        });

        masterForm.append(template);
    }

    function generateChildElements() {
        var template = "";
        $.each(interfaceConfigs.ChildGrids[0].Childs, function (i, value) {
            switch (value.EntryType) {
                case "text":
                    if (value.IsSys) {
                        template += '<div class="form-group">'
                            + '<label class="col-sm-3 control-label">' + value.Label + '</label>'
                            + '<div class="col-sm-5"><div class="input-group input-group-sm">'
                            + '<input type="text" class="form-control" id="' + value.ColumnName + '" name="' + value.ColumnName + '" readonly>'
                            + '<span class="input-group-btn">'
                            + '<button type="button" class="btn btn-default" id="ci-new-' + menuId + '"><i class="fa fa-plus" aria-hidden="true"></i></button>'
                            + '<button type="button" class="btn btn-success" id="ci-finder-' + menuId + '"><i class="fa fa-search" aria-hidden="true"></i></button>'
                            + '</span></div></div></div>';
                    }
                    else if (value.IsHidden && value.IsEnable) {
                        template += '<input type="hidden" id="' + value.ColumnName + '" name="' + value.ColumnName + '" value=""/>'
                    }
                    else {
                        template += '<div class="form-group">'
                            + '<label class="col-sm-3 control-label">' + value.Label + '</label>'
                            + '<div class="col-sm-5">'
                            + '<input type="text" class="form-control input-sm" id="' + value.ColumnName + '" name="' + value.ColumnName + '" placeholder="' + value.Label + '">'
                            + '</div></div>';
                    }
                    break;
                case "datetime":
                case "date":
                    template += '<div class="form-group">'
                        + '<label class="col-sm-3 control-label">' + value.Label + '</label>'
                        + '<div class="col-sm-5">'
                        + '<input type="text" class="form-control input-sm bootstrap-datepicker" id="' + value.ColumnName + '" name="' + value.ColumnName + '" placeholder="' + value.Label + '">'
                        + '</div></div>';
                    break;
                case "select":
                    template += '<div class="form-group">'
                        + '<label class="col-sm-3 control-label">' + value.Label + '</label>'
                        + '<div class="col-sm-5">'
                        + '<select class="form-control" id="' + value.ColumnName + '" name="' + value.ColumnName + '" style="width: 100%"></select>'
                        + '</div></div>';

                    var selectColumn = {};
                    selectColumn.id = value.ColumnName;
                    selectColumn.placeholder = value.Label;
                    selectColumn.apiUrl = value.SelectApiUrl;
                    selectColumn.hasDependentColumn = value.HasDependentColumn;
                    selectColumn.dependentColumnName = value.DependentColumnName;
                    selectColumn.defaultValue = value.DefaultValue;
                    childSelectColumnList.push(selectColumn);
                    break;
                case "number":
                    template += '<div class="form-group">'
                        + '<label class="col-sm-3 control-label">' + value.Label + '</label>'
                        + '<div class="col-sm-5">'
                        + '<input type="number" class="form-control input-sm" id="' + value.ColumnName + '" name="' + value.ColumnName + '" placeholder="' + value.Label + '">'
                        + '</div></div>';
                    break;
                case "checkbox":
                    template += '<div class="form-group"><div class="col-sm-offset-2 col-sm-10"><div class="checkbox checkbox-success">'
                        + '<input type="checkbox" id="c-' + value.ColumnName + '" name="' + value.ColumnName + '"> <label for="c-' + value.ColumnName + '">' + value.Label + '</label></div></div></div>';
                    break;
                default:
                    break;
            }

            if (value.IsRequired) {
                childConstraints[value.ColumnName] = value.MaxLength > 0 ? { presence: true, length: { maximum: value.MaxLength } } : { presence: true };
            }
        });

        childForm.append(template);
    }

    function initControls() {
        masterForm.find('.bootstrap-datepicker').datepicker({
            endDate: "0d",
            todayHighlight: true
        });
        
        $.each(selectColumnList, function (i, value) {
            var el = masterForm.find("#" + value.id);

            if (value.hasDependentColumn > 0) {
                masterForm.find("#" + value.dependentColumnName).on('select2:select', function (e) {
                    loadDependentSelection(el, value.apiUrl);
                });

                el.on('select2:select', function (e) {
                    setSelect2Combo(masterForm.find("#" + dependentSegmentEl.Id), e.params.data.desc)
                });
            }
            
            $.ajax({
                type: 'GET',
                url: value.apiUrl,
                async: false,
                success: function (data) {
                    el.select2({
                        placeholder: "Select " + value.placeholder,
                        allowClear: true,
                        data: data
                    });
                    el.val(null).trigger('change');
                },
                error: function () { console.log("Error in loading selection options") }
            });
        });

        $("#ci-new-" + menuId).click(function (e) {
            e.preventDefault();
            newId();
        });

        $("#ci-finder-" + menuId).click(function (e) {
            e.preventDefault();
            showFinderData();
        });
    }

    function initChildControls() {
        childForm.find('.bootstrap-datepicker').datepicker({
            endDate: "0d",
            todayHighlight: true
        });

        $.each(childSelectColumnList, function (i, value) {
            var el = childForm.find("#" + value.Id);

            //axios.get(value.SelectApiUrl)
            //    .then(function (response) {

            //    })
            //    .catch(function (error) {
            //        console.log(error);
            //    })

            if (value.hasDependentColumn > 0) {
                childForm.find("#" + value.dependentColumnName).on('select2:select', function (e) {
                    loadDependentSelection(el, value.apiUrl);
                });

                el.on('select2:select', function (e) {
                    setSelect2Combo($("#" + dependentSegmentEl.Id), e.params.data.desc)
                });
            }

            el.select2({
                placeholder: "Select " + value.placeholder,
                allowClear: true,
                data: value.List
            });
            el.val(null).trigger('change');
        });

        $("#ci-new-" + menuId).click(function (e) {
            e.preventDefault();
            newId();
        });

        $("#ci-finder-" + menuId).click(function (e) {
            e.preventDefault();
            showFinderData();
        });
    }

    function loadDependentSelection(el, apiUrl) {
        var url = apiUrl + "/" + id;
        axios.get(url)
            .then(function (response) {
                el.select2({
                    data: response.data
                });
            })
            .catch(function (error) {
                console.log(error.response)
            });
    }

    function generateChildGrid() {
        var childGridConfig = interfaceConfigs.ChildGrids[0];
        childGridApiUrl = childGridConfig.ApiUrl;
        var _columnNames = childGridConfig.ColumnNames.split(',');
        var _columnHeaders = childGridConfig.ColumnHeaders.split(',');
        var _columnAligns = childGridConfig.ColumnAligns.split(',');
        var _columnWidths = childGridConfig.ColumnWidths.split(',');
        var _hiddenColumns = childGridConfig.HiddenColumns.split(',');
        var _columnFilters = childGridConfig.ColumnFilters.split(',');
        var _columnTypes = childGridConfig.ColumnTypes.split(',');
        var _columnSortings = childGridConfig.ColumnSortings.split(',');

        var columnList = [];
        $.each(_columnNames, function (i, value) {
            var hidden = _hiddenColumns.find(function (element) {
                return element == value;
            });
            if (hidden)
                return true;
            var column = {
                field: value,
                title: _columnHeaders[i],
                align: _columnAligns[i],
                width: _columnWidths[i],
                sortable: convertToBoolean(_columnSortings[i])
            }

            if (convertToBoolean(_columnFilters[i])) {
                column["filterControl"] = "input"
            }
            if (_columnTypes[i] == 'checkbox') {
                column["formatter"] = function (v) {
                    return v ? '<span class="bg-success">Yes<span>' : '<span class="bg-warning">No<span>'
                }
            }

            columnList.push(column);
        });

        var actionColumn = {
            title: 'Actions',
            align: 'center',
            width: 100,
            formatter: function () {
                return [
                    '<span class="btn-group">',
                    '<a class="btn btn-success btn-xs edit" href="javascript:void(0)" title="Edit Item">',
                    '<i class="fa fa-edit"></i>',
                    '</a>',
                    '<a class="btn btn-success btn-xs remove" href="javascript:void(0)" title="Delete Item">',
                    '<i class="fa fa-trash"></i>',
                    '</a>',
                    '</span>'
                ].join('');
            },
            events: {
                'click .edit': function (e, value, row, index) {
                    setChildData(row);
                },
                'click .remove': function (e, value, row, index) {
                    e.stopPropagation();
                    var itemIndex = draftChilds.rows.findIndex(function (element) {
                        return element.Id == row.Id;
                    });

                    if (!row.Id || row.Id <= 0) {
                        draftChilds.rows.splice(itemIndex, 1);
                        draftChilds.total = draftChilds.rows.length;
                        $("#tabaleGridData-" + menuId).bootstrapTable('load', draftChilds);
                    }
                    else {
                        draftChilds.rows[itemIndex].EntityState = 8;
                        var $target = $(e.target);
                        $target.closest("tr").addClass('deleted-row');
                    }
                }
            }
        }
        columnList.push(actionColumn);

        $("#tabaleGridData-" + menuId).bootstrapTable({
            pagination: true,
            filterControl: true,
            searchOnEnterKey: true,
            sidePagination: "server",
            pageList: "[10, 25, 50, 100, 500]",
            cache: false,
            columns: columnList,
            onPageChange: function (number, size) {
                var newOffset = (number - 1) * size;
                var newLimit = size;
                if (childGridTableParams.offset == newOffset && childGridTableParams.limit == newLimit)
                    return;

                childGridTableParams.offset = newOffset;
                childGridTableParams.limit = newLimit;

                getGridData();
            },
            onSort: function (name, order) {
                childGridTableParams.sort = name;
                childGridTableParams.order = order;
                childGridTableParams.offset = 0;

                getGridData();
            },
            onRefresh: function () {
                childGridTableParams.offset = 0;
                childGridTableParams.limit = 10;
                childGridTableParams.sort = '';
                childGridTableParams.order = '';
                childGridTableParams.filter = '';

                getGridData();
            },
            onColumnSearch: function (columnName, filterValue) {
                if (columnName in filterBy && !filterValue) {
                    delete filterBy[columnName];
                }
                else
                    filterBy[columnName] = filterValue;

                if (Object.keys(filterBy).length === 0 && filterBy.constructor === Object)
                    childGridTableParams.filter = "";
                else
                    childGridTableParams.filter = JSON.stringify(filterBy);

                getGridData();
            }
        });
    }

    function getGridData() {
        var queryParams = $.param(childGridTableParams);
        var url = childGridApiUrl + "?" + queryParams;
        axios.get(url)
            .then(function (response) {
                $("#tabaleGridData-" + menuId).bootstrapTable('load', response.data);
            })
            .catch(function () {
                toastr.error(constants.LOAD_ERROR_MESSAGE);
            })
    }
    // #endregion

    // #region Finder Section
    function showFinderData() {
        var finderData = interfaceConfigs.Childs.find(function (element) {
            return element.HasFinder == true;
        });
        finderApiUrl = finderData.FinderApiUrl;

        var _columnsFields = finderData.FinderHeaderColumns.split(',');
        var _columnTitles = finderData.FinderDisplayColumns.split(',');
        var _columnAligns = finderData.FinderColumnAligns.split(',');
        var _columnWidths = finderData.FinderColumnWidths.split(',');
        var _columnSortings = finderData.FinderColumnSortings.split(',');
        var _columnFilters = finderData.FinderFilterColumns.split(',');

        var columnList = [];
        $.each(_columnsFields, function (i, value) {
            var column = {
                field: value,
                title: _columnTitles[i],
                align: _columnAligns[i],
                width: _columnWidths[i]

                //Sorting ignored because it was not working properly
                //sortable: convertToBoolean(_columnSortings[i])
            };
            if (convertToBoolean(_columnFilters[i])) {
                column["filterControl"] = "input";
            }
            columnList.push(column);
        });
        
        $("#tblSearchData-" + menuId).bootstrapTable({
            pagination: true,
            filterControl: true,
            searchOnEnterKey: true,
            sidePagination: "server",
            pageList: "[10, 25, 50, 100, 500]",
            cache: false,
            columns: columnList,
            onDblClickRow: function (row, $element, field) {
                var url = interfaceConfigs.ApiUrl + "/" + row.Id;
                axios.get(url)
                    .then(function (response) {
                        setMasterData(response.data);
                        $("#modal-finder-" + menuId).modal('hide');
                    })
                    .catch(function (error) {
                        console.log(error.response.data);
                        toastr.error(error.response.data.Message)
                    });
            },            
            onPageChange: function (number, size) {
                var newOffset = (number - 1) * size;
                var newLimit = size;
                if (finderTableParams.offset == newOffset && finderTableParams.limit == newLimit)
                    return;
                finderTableParams.offset = newOffset;
                finderTableParams.limit = newLimit;

                getFinderData();
            },
            onSort: function (name, order) {
                finderTableParams.sort = name;
                finderTableParams.order = order;
                finderTableParams.offset = 0;

                getFinderData();
            },
            onRefresh: function () {
                finderTableParams.offset = 0;
                finderTableParams.limit = 10;
                finderTableParams.sort = '';
                finderTableParams.order = '';
                finderTableParams.filter = '';

                getFinderData();
            },
            onColumnSearch: function (columnName, filterValue) {
                if (columnName in filterBy && !filterValue) {
                    delete filterBy[columnName];
                }

                if (filterValue)
                    filterBy[columnName] = filterValue;

                if (Object.keys(filterBy).length === 0 && filterBy.constructor === Object)
                    finderTableParams.filter = "";
                else
                    finderTableParams.filter = JSON.stringify(filterBy);

                getFinderData();
            }
        });

        $(".filter-control input").attr('placeholder', 'Type & Enter for Search');
        $(".filter-control input").css('border', '1px solid gray');
        $(".filter-control input").css('font-size', '13px');

        axios.get(finderData.FinderApiUrl)
            .then(function (response) {
                $("#tblSearchData-" + menuId).bootstrapTable('load', response.data);
                $("#modal-finder-" + menuId).modal("show");
            })
            .catch(function (error) {
                toastr.error(constants.LOAD_ERROR_MESSAGE);
            });
    }

    function getFinderData() {
        var queryParams = $.param(finderTableParams);
        var url = finderApiUrl + "?" + queryParams;
        axios.get(url)
            .then(function (response) {                
                $("#tblSearchData-" + menuId).bootstrapTable('load', response.data);
            })
            .catch(function () {
                toastr.error(constants.LOAD_ERROR_MESSAGE);
            })
    }
    // #endregion

    function resetForm() {
        masterForm.trigger("reset");

        $.each(interfaceConfigs.Childs, function (i, value) {
            if (value.EntryType == "select")
                $('#' + value.ColumnName).val('').trigger('change');
        });

        masterForm.find("#Id").val(-1111);
        masterForm.find("#EntityState").val(4);

        resetChildForm();
        $("#tabaleGridData-" + menuId).bootstrapTable("removeAll");
    }

    function resetChildForm() {
        childForm.trigger("reset");
        childForm.find("#Id").val(-1111);
        childForm.find("#EntityState").val(4);
    }

    function newId() {
        resetForm();
        masterForm.find("#Id").val(-1111);
    }

    // #region Save
    function saveMaster() {
        var l = $('#btnSaveMaster').ladda();
        l.ladda('start');

        if (!validateMasterForm()) {
            l.ladda('stop');
            return;
        }

        $('#form-ci-' + menuId).find(':checkbox').each(function () {
            this.value = this.checked;
        });

        var data = formDataToJson(masterForm.serializeArray());
        data["Childs"] = draftChilds.rows;

        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        }
        axios.post(interfaceConfigs.ApiUrl, data, config)
            .then(function () {
                toastr.success(constants.SUCCESS_MESSAGE);
                l.ladda('stop');
                resetForm();
                draftChilds.rows = [];
                draftChilds.total = 0;
                $("#tabaleGridData-" + menuId).bootstrapTable('load', draftChilds);
            })
            .catch(function (error) {
                toastr.error(error.response.data.Message);
            });
    }

    function validateMasterForm() {
        initializeValidation(masterForm, constraints);

        if (!isValidForm(masterForm, constraints)) {
            toastr.error("Please correct all validation ")
            return false;
        }
        else {
            hideValidationErrors(masterForm);
            return true;
        }
    }

    function saveChild() {
        var l = $("#btnSaveChild-" + menuId).ladda();
        l.ladda('start');

        if (!validateChildForm()) {
            l.ladda('stop');
            return;
        }

        $('#form-ci-child-' + menuId).find(':checkbox').each(function () {
            this.value = this.checked;
        });

        var formData = childForm.serializeArray();
        var data = {};
        $.each(formData, function (i, v) {
            data[v.name] = v.value;
            data["Actions"] = "";
        });

        data[interfaceConfigs.ChildGrids[0].ParentColumn] = masterForm.find("#Id").val();

        draftChilds.rows.push(data);
        draftChilds.total = draftChilds.rows.length;

        $("#tabaleGridData-" + menuId).bootstrapTable('load', draftChilds);

        l.ladda('stop');
        $("#modal-child-" + menuId).modal('hide');
    }

    function validateChildForm() {
        initializeValidation(childForm, childConstraints);

        if (!isValidForm(childForm, childConstraints)) {
            toastr.error("Please correct all validation ")
            return false;
        }
        else {
            hideValidationErrors(childForm);
            return true;
        }
    }
    // #endregion

    // #region SetData in Control
    function setMasterData(data) {
        $.each(interfaceConfigs.Childs, function (i, value) {
            switch (value.EntryType) {
                case "checkbox":
                    masterForm.find('#' + value.ColumnName).prop('checked', data[value.ColumnName]);
                    break;
                case "radio":
                    masterForm.find('input[name="' + value.ColumnName + '"]').val([data[value.ColumnName]]);
                    break;
                case "select":
                    masterForm.find('#' + value.ColumnName).val(data[value.ColumnName]).trigger("change");
                    break;
                default:
                    masterForm.find('#' + value.ColumnName).val(data[value.ColumnName]);
                    break;
            }
        });

        if (interfaceConfigs.HasGrid) {
            draftChilds.rows = data.Childs;
            draftChilds.total = data.Childs.length;
            $("#tabaleGridData-" + menuId).bootstrapTable('load', data.Childs);
        }
    }

    function setChildData(data) {
        $.each(interfaceConfigs.ChildGrids[0].Childs, function (i, value) {
            switch (value.EntryType) {
                case "checkbox":
                    childForm.find("input[name=" + value.ColumnName + "]").prop('checked', data[value.ColumnName]);
                    break;
                case "radio":
                    childForm.find("input[name=" + value.ColumnName + "]").val([data[value.ColumnName]]);
                    break;
                case "select":
                    childForm.find("input[name=" + value.ColumnName + "]").val(data[value.ColumnName]).trigger("change");
                    break;
                default:
                    childForm.find("input[name=" + value.ColumnName + "]").val(data[value.ColumnName]);
                    break;
            }
        });

        $("#modal-child-" + menuId).modal('show');
    }
    // #endregion
})();