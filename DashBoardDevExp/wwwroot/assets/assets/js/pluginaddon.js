/**
* Common plugins
*/
/**
 * Toast UI Notification
 */
var toastExamples = document.querySelectorAll("[data-toast]");
toastExamples.forEach(function (element) {
    element.addEventListener("click", function () {
        var toastData = {};
        var isToastVal = element.attributes;
        if (isToastVal["data-toast-text"]) {
            toastData.text = isToastVal["data-toast-text"].value.toString();
        }
        if (isToastVal["data-toast-gravity"]) {
            toastData.gravity = isToastVal["data-toast-gravity"].value.toString();
        }
        if (isToastVal["data-toast-position"]) {
            toastData.position = isToastVal["data-toast-position"].value.toString();
        }
        if (isToastVal["data-toast-className"]) {
            toastData.className = isToastVal["data-toast-className"].value.toString();
        }
        if (isToastVal["data-toast-duration"]) {
            toastData.duration = isToastVal["data-toast-duration"].value.toString();
        }
        if (isToastVal["data-toast-close"]) {
            toastData.close = isToastVal["data-toast-close"].value.toString();
        }
        if (isToastVal["data-toast-style"]) {
            toastData.style = isToastVal["data-toast-style"].value.toString();
        }
        if (isToastVal["data-toast-offset"]) {
            toastData.offset = isToastVal["data-toast-offset"]
        }
        Toastify({
            newWindow: true,
            text: toastData.text,
            gravity: toastData.gravity,
            position: toastData.position,
            className: "bg-" + toastData.className,
            stopOnFocus: true,
            offset: {
                x: toastData.offset ? 50 : 0, // horizontal axis - can be a number or a string indicating unity. eg: '2em'
                y: toastData.offset ? 10 : 0, // vertical axis - can be a number or a string indicating unity. eg: '2em'
            },
            duration: toastData.duration,
            close: toastData.close == "close" ? true : false,
            style: toastData.style == "style" ? {
                background: "linear-gradient(to right, #0AB39C, #405189)",
            } : "",
        }).showToast();
    });
});

/**
 * Choices Select plugin
 */
var choicesExamples = document.querySelectorAll("[data-choices]");
choicesExamples.forEach(function (item) {
    var choiceData = {};
    var isChoicesVal = item.attributes;
    if (isChoicesVal["data-choices-groups"]) {
        (choiceData.placeholderValue = "This is a placeholder set in the config")
    }
    if (isChoicesVal["data-choices-search-false"]) {
        (choiceData.searchEnabled = false)
    }
    if (isChoicesVal["data-choices-search-true"]) {
        (choiceData.searchEnabled = true)
    }
    if (isChoicesVal["data-choices-removeItem"]) {
        (choiceData.removeItemButton = true)
    }
    if (isChoicesVal["data-choices-sorting-false"]) {
        choiceData.shouldSort = false;
    }
    if (isChoicesVal["data-choices-sorting-true"]) {
        choiceData.shouldSort = true;
    }
    if (isChoicesVal["data-choices-multiple-default"]) {
        // choiceData.removeItemButton=true
    }
    if (isChoicesVal["data-choices-multiple-groups"]) {
        // choiceData.removeItemButton=true
    }
    if (isChoicesVal["data-choices-multiple-remove"]) {
        choiceData.removeItemButton = true;
    }
    if (isChoicesVal["data-choices-limit"]) {
        choiceData.maxItemCount = isChoicesVal["data-choices-limit"].value.toString();
    }
    if (isChoicesVal["data-choices-limit"]) {
        choiceData.maxItemCount = isChoicesVal["data-choices-limit"].value.toString();
    }
    if (isChoicesVal["data-choices-editItem-true"]) {
        choiceData.maxItemCount = true
    }
    if (isChoicesVal["data-choices-editItem-false"]) {
        choiceData.maxItemCount = false
    }
    if (isChoicesVal["data-choices-text-unique-true"]) {
        (choiceData.duplicateItemsAllowed = false)
    }
    if (isChoicesVal["data-choices-text-disabled-true"]) {
        (choiceData.addItems = false)
    }
    isChoicesVal["data-choices-text-disabled-true"] ?
        new Choices(item, choiceData).disable() :
        new Choices(item, choiceData);
});

/**
 * flatpickr
 */
var flatpickrExamples = document.querySelectorAll("[data-provider]");
flatpickrExamples.forEach(function (item) {
    if (item.getAttribute("data-provider") == "flatpickr") {
        var dateData = {};
        var isFlatpickerVal = item.attributes;
        if (isFlatpickerVal["data-date-format"]) {
            (dateData.dateFormat = isFlatpickerVal["data-date-format"].value.toString());
        }
        if (isFlatpickerVal["data-enable-time"]) {
            dateData.enableTime = true,
                dateData.dateFormat = isFlatpickerVal["data-date-format"].value.toString() + " H:i";
        }
        if (isFlatpickerVal["data-altFormat"]) {
            dateData.altInput = true,
                dateData.altFormat = isFlatpickerVal["data-altFormat"].value.toString();
        }
        if (isFlatpickerVal["data-minDate"]) {
            dateData.minDate = isFlatpickerVal["data-minDate"].value.toString();
            dateData.dateFormat = isFlatpickerVal["data-date-format"].value.toString();
        }
        if (isFlatpickerVal["data-maxDate"]) {
            dateData.maxDate = isFlatpickerVal["data-maxDate"].value.toString();
            dateData.dateFormat = isFlatpickerVal["data-date-format"].value.toString();
        }
        if (isFlatpickerVal["data-deafult-date"]) {
            dateData.defaultDate = isFlatpickerVal["data-deafult-date"].value.toString();
            dateData.dateFormat = isFlatpickerVal["data-date-format"].value.toString();
        }
        if (isFlatpickerVal["data-multiple-date"]) {
            dateData.mode = "multiple";
            dateData.dateFormat = isFlatpickerVal["data-date-format"].value.toString();
        }
        if (isFlatpickerVal["data-range-date"]) {
            dateData.mode = "range";
            dateData.dateFormat = isFlatpickerVal["data-date-format"].value.toString();
        }
        if (isFlatpickerVal["data-inline-date"]) {
            dateData.inline = true,
                dateData.defaultDate = isFlatpickerVal["data-deafult-date"].value.toString();
            dateData.dateFormat = isFlatpickerVal["data-date-format"].value.toString();
        }
        if (isFlatpickerVal["data-disable-date"]) {
            var dates = []
            dates.push(isFlatpickerVal["data-disable-date"].value);
            dateData.disable = dates.toString().split(",");
        }
        flatpickr(item, dateData);
    } else if (item.getAttribute("data-provider") == "timepickr") {
        var timeData = {};
        var isTimepickerVal = item.attributes;
        if (isTimepickerVal["data-time-basic"]) {
            (timeData.enableTime = true),
                (timeData.noCalendar = true),
                (timeData.dateFormat = "H:i");
        }
        if (isTimepickerVal["data-time-hrs"]) {
            (timeData.enableTime = true),
                (timeData.noCalendar = true),
                (timeData.dateFormat = "H:i"),
                (timeData.time_24hr = true);
        }
        if (isTimepickerVal["data-min-time"]) {
            (timeData.enableTime = true),
                (timeData.noCalendar = true),
                (timeData.dateFormat = "H:i"),
                timeData.minTime = isTimepickerVal["data-min-time"].value.toString()
        }
        if (isTimepickerVal["data-max-time"]) {
            (timeData.enableTime = true),
                (timeData.noCalendar = true),
                (timeData.dateFormat = "H:i"),
                timeData.minTime = isTimepickerVal["data-max-time"].value.toString()
        }
        if (isTimepickerVal["data-default-time"]) {
            (timeData.enableTime = true),
                (timeData.noCalendar = true),
                (timeData.dateFormat = "H:i"),
                (timeData.defaultDate = isTimepickerVal["data-default-time"].value.toString());
        }
        if (isTimepickerVal["data-time-inline"]) {
            (timeData.enableTime = true),
                (timeData.noCalendar = true),
                (timeData.defaultDate = isTimepickerVal["data-time-inline"].value.toString());
            (timeData.inline = true);
        }
        flatpickr(item, timeData);
    }
});

// Dropdown
document.querySelectorAll('.dropdown-menu a[data-bs-toggle="tab"]').forEach(function (element) {
    element.addEventListener('click', function (e) {
        e.stopPropagation();
        bootstrap.Tab.getInstance(e.target).show();
    });
});