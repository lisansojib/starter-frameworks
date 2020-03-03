'use strict'
function makeArray(stringData) {
    return $.parseJSON(stringData);
}

String.prototype.replaceAll = function (stringToFind, stringToReplace) {
    if (stringToFind === stringToReplace) return this;
    var temp = this;
    var index = temp.indexOf(stringToFind);
    while (index != -1) {
        temp = temp.replace(stringToFind, stringToReplace);
        index = temp.indexOf(stringToFind);
    }
    return temp;
};

function showBootboxConfirm(title, message, callback) {
    bootbox.confirm({
        title: title,
        message: message,
        size: "small",
        buttons: {
            confirm: {
                label: 'Yes',
                className: 'btn-success'
            },
            cancel: {
                label: 'No',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            return callback(result);
        }
    });
}

function showBootboxPrompt(title, message, callback) {
    bootbox.prompt({
        title: title,
        message: "<p>" + message + "</p>",
        buttons: {
            confirm: {
                label: 'Yes',
                className: 'btn-success'
            },
            cancel: {
                label: 'No',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            return callback(result);
        }
    });
}

/**
 * Show bootbox.js Select Prompt
 * @param {any} title - Prompt Title
 * @param {any} optionsArray - Input Options Array
 * @param {any} size - Prompt Size
 * @param {any} callback - Callback function
 */
function showBootboxSelectPrompt(title, optionsArray, size, callback) {
    if (!size) size = "small";
    bootbox.prompt({
        title: title,
        size: size,
        inputType: 'select',
        inputOptions: optionsArray,
        callback: function (result) {
            return callback(result);
        }
    });
}

/**
 * Shows Sweet Alert Confirm Box
 * @param {string} title - Your confirm title
 * @param {string} message - Your confirm message
 * @param {Function} callback - Your callback function
 */
function sweetConfirmBox(title, message, callback) {
    swal({
        title: title,
        text: message,
        icon: "info",
        buttons: true
    }).then(function (yes) {
        return callback(yes);
    });
}

/**
 * Shows Sweet Alert Confirm Box
 * @param {string} title - Your confirm title
 * @param {Function} callback - Your callback function
 */
function sweetConfirmBoxWithPrompt(title, callback) {
    swal(title, {
        content: "input"
    })
    .then(function(value){
        return callback(value);
    });
}

/**
 * Show Sweet Alert Success Box
 * @param {any} title - Your confirm title
 * @param {any} message - Your confirm message
 */
function sweetSuccessBox(title, message) {
    swal({
        title: title,
        text: message,
        icon: "success"
    });
}

/**
 * Preview on image load
 * @param {Event} event - onchange evnet
 * @param {HTMLImageElement} previewImgId - Preview image id
 */
function previewImage(event, previewImgId) {
    var preview = document.getElementById(previewImgId);
    var file = event.target.files[0];
    var reader = new FileReader();

    reader.addEventListener("load", function () {
        preview.src = reader.result;
    }, false);

    if (file) {
        reader.readAsDataURL(file);
    }
}

/**
 * Converts a string to boolean
 * @param {string} str - input string
 */
function convertToBoolean(str) {
    if (!str)
        return false;

    switch (str.toLowerCase()) {
        case "true":
        case "yes":
        case "1":
            return true;
        default: return false;
    }
}

function formDataToJson(data) {
    var jsonObj = {};
    $.each(data, function (i, v) {
        jsonObj[v.name] = v.value;
    });

    return jsonObj;
}

/**
 * Set Form Data
 * @param {any} $formEl - Form Element
 * @param {any} data - data object
 */
function setFormData($formEl, data) {
    if (!data && ! typeof data === 'object') {
        console.error("Your data is not valid.");
        return;
    }

    try {
        $formEl.find("input, select").each(function () {
            try {
                var $input = $(this);
                var value = data[$input.attr('name')];
                if (!value) return;

                switch ($input.attr('type')) {
                    case "checkbox":
                        $input.prop("checked", value);
                        break;
                    case "radio":
                        $input.each(function (i) {
                            if ($(this).val() == value) $(this).attr({
                                checked: true
                            })
                        });
                        break;
                    case undefined:
                        if ($input.hasClass("select2-hidden-accessible")) {
                            var optionListName = $input[0].multiple ? $input.attr('name').replace("Ids", '') : $input.attr('name').replace("Id", '');
                            optionListName += "List";
                            initSelect2($input, data[optionListName]);
                            $input.val(value).trigger("change");
                        }
                        break;
                    case "file":
                        break;
                    default:
                        $input.val(value);
                        break;
                }
            } catch (e) {
                console.error(e);
            }
        });
    } catch (e) {
        console.error(e);
    }
}

/**
 * Disable elemnt
 * @param {HTMLElement} el - HTMLElement
 */
function disableElement(el) {
    el.attr('disabled', 'disabled');
}

/**
 * Enable element
 * @param {HTMLElement} el - HTMLElement
 */
function enableElement(el) {
    el.removeAttr('disabled');
}

/**
 * Makes an input readonly
 * @param {HTMLInputElement} el - Input element
 */
function makeReadonly(el) {
    el.prop("readonly", true);
}

/**
 * Removes readonly attribute
 * @param {HTMLInputElement} el - Input element
 */
function removeReadonly(el) {
    el.prop("readonly", false);
}

/**
 * Check a checkbox element
 * @param {HTMLInputElement} el - Checkbox Element
 */
function checkInput(el) {
    el.prop("checked", true);
}

function setCheckBox(el, value) {
    el.prop("checked", value);
}

/**
 * Un-check a checkbox element
 * @param {HTMLInputElement} el - Checkbox Element
 */
function unCheckInput(el) {
    el.prop("checked", false);
}

/**
 * Returned if checkbox is checked or not
 * @param {HTMLInputElement} el - Checkbox Element
 */
function isChecked(el) {
    return el.is(':checked')
}

/**
 * Sets Select2 Combo
 * @param {HTMLInputElement} el - Select Element
 */
function setSelect2Combo(el, val) {
    el.val(val).trigger("change");
}

/**
 * Resets Select2 Combo
 * @param {HTMLInputElement} el - Select Element
 */
function resetSelect2Combo(el) {
    el.val(null).trigger("change");
}

/**
 * Convert Select2 Array to Select Array
 * @param {Array} srcArray - Select2 Array
 */
function convertToSelectOptions(srcArray) {
    return srcArray.map(function(obj) {
        return { value: obj.id, text: obj.text };
    });
}

function filterPlaceholderControl() {
    $(".filter-control input").attr('placeholder', 'Type & Enter for Search');
    $(".filter-control input").css('border', '1px solid gray');
    $(".filter-control input").css('font-size', '11px');
}

function setMultiSelectValueInBootstrapTableEditable(list, value, $el) {
    var selectedNameIds = _.filter(_.uniq(value.split(',')), function (value) {
        return value > 0;
    });

    var selectedListItem = _.filter(list, function (i) { return this.values.indexOf(i.id) > -1; }, { "values": selectedNameIds });
    var selectedText = _.map(selectedListItem, function (el) { return el.text }).join(",");
    $($el).text(selectedText);
    $($el).editable('setValue', selectedText);
    return { id: selectedNameIds.toString(), text: selectedText};
}

function setMultiSelectValueByValueInBootstrapTableEditable(list, value, $el) {
    var selectedNameIds = _.filter(_.uniq(value.split(',')), function (value) {
        return value > 0;
    });

    var selectedListItem = _.filter(list, function (i) { return this.values.indexOf(i.value) > -1; }, { "values": selectedNameIds });
    var selectedText = _.map(selectedListItem, function (el) { return el.text }).join(",");
    $($el).text(selectedText);
    $($el).editable('setValue', selectedText);
    return { id: selectedNameIds.toString(), text: selectedText };
}

/**
 * Initialize Select2
 * @param {HTMLSelectElement} Select Element
 * @param {Array} Array of Select2 Data
 * @param {Boolean} Allow Clear
 * @param {String} Placeholder
 * @param {Boolean} Show Default Option
 */
function initSelect2($el, data, allowClear = true, placeholder = "Select a Value", showDefaultOption = true) {
    if (showDefaultOption)
        data.unshift({ id: '', text: '' });

    $el.html('').select2({ 'data': data, 'allowClear': allowClear, 'placeholder': placeholder });
}

/**
 *
 * @param {Array} Array Data
 * @param {string} Filter By Array element
 */
function getMaxIdForArray(data, filterBy) {
    if (data.length == 0)
        return 1;
    if (!filterBy)
        throw "Filter By is required.";

    var maxId = Math.max.apply(Math, data.map(function (el) { return el[filterBy]; }));
    return ++maxId;
}

function setSelect2Data($el, value) {
    if (value)
        $el.val(value).trigger('change');
}

/**
 * 
 * @param {HTMLButtonElement} el - HTML Button element
 * @param {HTMLDivElement} $parentEl - Jquery Div element
 */
function toggleActiveToolbarBtn(el, $parentEl) {
    $parentEl.find('button').not("#" + el.id).removeClass("btn-success").addClass("btn-default text-grey");

    if (el instanceof jQuery)
        el.removeClass("btn-default text-grey").addClass("btn-success text-white");
    else
        $(el).removeClass("btn-default text-grey").addClass("btn-success text-white");
}

// #region Validation
/**
 * Initialize validation on the container
 * @param {HTMLDivElement | HTMLFormElement} container - form container
 * @param {Array} constraints - Array of Constraints
 */
function initializeValidation(container, constraints) {
    extendValidation();
    var inputs = container.find("input[name], select[name]");
    for (var i = 0; i < inputs.length; ++i) {
        inputs[i].addEventListener("change", function (ev) {
            var errors = validate(container, constraints) || {};
            showErrorsForInput(this, errors[this.name])
        });
    }
}

/**
 * Validate the container
 * @param {HTMLDivElement | HTMLFormElement} container - form container
 * @param {Array} constraints - Array of Constraints
 */
function isValidForm(container, constraints) {
    var errors = validate(container, constraints);

    if (!errors)
        return true;
    else showErrors(container, errors || {});
}

/**
 * Show errors to the form container
 * Loops through all the inputs and show the errors for that input
 * @param {HTMLDivElement | HTMLFormElement} container
 * @param {Array} errors - Array of errors
 */
function showErrors(container, errors) {
    var inputs = container.find("input[name], select[name]");
    $.each(inputs, function (i, value) {
        showErrorsForInput(value, errors && errors[value.name]);
    });
}

/**
 * shows/hide validation error for the input
 * @param {HTMLInputElement} input - HTMLInputElement
 * @param {string} error - Error message
 */
function showErrorsForInput(input, error) {
    var formGroup = $(input).closest('.form-group');

    if (!formGroup) return;

    // remove error messages
    formGroup.removeClass('has-error');
    $("#validation-message-" + input.id).remove();

    if (error === undefined || error.length === 0) return;

    formGroup.addClass('has-error');

    // add help-block
    var message = document.createElement('span');
    message.className = 'help-block validation-message';
    message.textContent = error;
    message.id = 'validation-message-' + input.id;

    // check if input block
    if ($(input).parent().hasClass("input-group"))
        $(input).closest('.input-group').parent().append(message);
    else
        $(input).after(message);
}

/**
 * Hides all validation error
 *  @param {HTMLDivElement | HTMLFormElement} container - form container
 */
function hideValidationErrors(container) {
    var inputs = container.find("input[name], select[name]");

    container.find('.help-block').remove();

    $.each(inputs, function (i, value) {
        var formGroup = $(value).closest('.form-group');
        if (!formGroup) return;
        formGroup.removeClass('has-error');
    });
}

function extendValidation() {
    // Before using it we must add the parse and format functions
    // Here is a sample implementation using moment.js
    validate.extend(validate.validators.datetime, {
        // The value is guaranteed not to be null or undefined but otherwise it
        // could be anything.
        parse: function (value, options) {
            return +moment.utc(value);
        },
        // Input is a unix timestamp
        format: function (value, options) {
            var format = options.dateOnly ? "YYYY-MM-DD" : "YYYY-MM-DD hh:mm:ss";
            return moment.utc(value).format(format);
        }
    });
}
// #endregion

// #region Polyfills
// https://tc39.github.io/ecma262/#sec-array.prototype.find
if (!Array.prototype.find) {
    Object.defineProperty(Array.prototype, 'find', {
        value: function (predicate) {
            // 1. Let O be ? ToObject(this value).
            if (this == null) {
                throw new TypeError('"this" is null or not defined');
            }

            var o = Object(this);

            // 2. Let len be ? ToLength(? Get(O, "length")).
            var len = o.length >>> 0;

            // 3. If IsCallable(predicate) is false, throw a TypeError exception.
            if (typeof predicate !== 'function') {
                throw new TypeError('predicate must be a function');
            }

            // 4. If thisArg was supplied, let T be thisArg; else let T be undefined.
            var thisArg = arguments[1];

            // 5. Let k be 0.
            var k = 0;

            // 6. Repeat, while k < len
            while (k < len) {
                // a. Let Pk be ! ToString(k).
                // b. Let kValue be ? Get(O, Pk).
                // c. Let testResult be ToBoolean(? Call(predicate, T, « kValue, k, O »)).
                // d. If testResult is true, return kValue.
                var kValue = o[k];
                if (predicate.call(thisArg, kValue, k, o)) {
                    return kValue;
                }
                // e. Increase k by 1.
                k++;
            }

            // 7. Return undefined.
            return undefined;
        },
        configurable: true,
        writable: true
    });
}

// https://tc39.github.io/ecma262/#sec-array.prototype.findindex
if (!Array.prototype.findIndex) {
    Object.defineProperty(Array.prototype, 'findIndex', {
        value: function (predicate) {
            // 1. Let O be ? ToObject(this value).
            if (this == null) {
                throw new TypeError('"this" is null or not defined');
            }

            var o = Object(this);

            // 2. Let len be ? ToLength(? Get(O, "length")).
            var len = o.length >>> 0;

            // 3. If IsCallable(predicate) is false, throw a TypeError exception.
            if (typeof predicate !== 'function') {
                throw new TypeError('predicate must be a function');
            }

            // 4. If thisArg was supplied, let T be thisArg; else let T be undefined.
            var thisArg = arguments[1];

            // 5. Let k be 0.
            var k = 0;

            // 6. Repeat, while k < len
            while (k < len) {
                // a. Let Pk be ! ToString(k).
                // b. Let kValue be ? Get(O, Pk).
                // c. Let testResult be ToBoolean(? Call(predicate, T, « kValue, k, O »)).
                // d. If testResult is true, return k.
                var kValue = o[k];
                if (predicate.call(thisArg, kValue, k, o)) {
                    return k;
                }
                // e. Increase k by 1.
                k++;
            }

            // 7. Return -1.
            return -1;
        },
        configurable: true,
        writable: true
    });
}

// Search substring in a string
if (!String.prototype.includes) {
    String.prototype.includes = function (search, start) {
        'use strict';
        if (typeof start !== 'number') {
            start = 0;
        }

        if (start + search.length > this.length) {
            return false;
        } else {
            return this.indexOf(search, start) !== -1;
        }
    };
}
// #endregion