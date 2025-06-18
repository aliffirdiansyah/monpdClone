var pemkot = {
    //global variable
    //redirectUrlError: '/Home/Error',
    swalFireIcon: {
        success: 'success',
        warning: 'warning',
        info: 'info',
    },
    blockUi: function () {
        $("#loader").show();
    },
    unBlockUi: function () {
        $("#loader").hide();
    },
    swalfireSuccessWithRefresh: function (message, callback = null) {
        Swal.fire({
            icon: 'success',
            title: '<b>Info</b>',
            text: message,
        }).then((result) => {
            if (callback != null) {
                callback();
            }
            else {
                window.location.reload()
            }
        })
    },
    swalfireSuccess: (message) => {
        Swal.fire({
            icon: 'success',
            title: '<b>Info</b>',
            text: message,
        })
    },
    swalFireError: (message) => {
        Swal.fire({
            icon: pemkot.swalFireIcon.error,
            title: '<b style="color:orange">Peringatan</b>',
            text: message,
        })
    },
    swalFireInfo: (message) => {
        Swal.fire({
            icon: pemkot.swalFireIcon.info,
            title: '<b>Info</b>',
            text: message,
        })
    },
    ajaxformFile: (url, formData = null, verificationToken = null, callback = null, callbackError = null) => {
        $.ajax({
            url: url,
            type: "POST",
            data: formData,
            dataType: "json",
            contentType: false,
            processData: false,
            headers:
            {
                "RequestVerificationToken": verificationToken
            },
            beforeSend: function () {
                $("#loader").show();
            },
            complete: function () { $("#loader").hide(); },
            success: function (msg) {
                if (callback == null) {
                    pemkot.swalfireSuccessWithRefresh("Success");
                }
                else {
                    callback(msg)
                }
            },
            error: (xhr, status, err) => {
                if (callbackError == null) {
                    pemkot.swalFireError(xhr.responseText);
                }
                else {
                    callbackError(xhr);
                }
            }
        })
    },
    ajax: (url, formData = null, verificationToken = null, callback = null, callbackError = null) => {
        $.ajax({
            url: url,
            type: "POST",
            data: formData,
            dataType: "json",
            headers:
            {
                "RequestVerificationToken": verificationToken
            },
            beforeSend: function () {
                $("#loader").show();
            },
            complete: function () { $("#loader").hide(); },
            success: function (msg) {
                if (callback == null) {
                    pemkot.swalfireSuccessWithRefresh("Success");
                }
                else {
                    callback(msg)
                }
            },
            error: (xhr, status, err) => {
                if (callbackError == null) {
                    pemkot.swalFireError(xhr.responseText);
                }
                else {
                    callbackError(xhr);
                }
            }
        })
    },
    ajaxWithFile: (url, formData = null, verificationToken = null, callback = null, callbackError = null, withConfirmation = true) => {

        if (withConfirmation) {
            Swal.fire({
                icon: pemkot.swalFireIcon.info,
                title: 'Confirmation',
                text: 'Do you want to save the changes?',
                showDenyButton: true,
                confirmButtonText: 'Yes',
                denyButtonText: 'No'
            }).then((result) => {
                if (result.isConfirmed) {
                    pemkot.ajaxformFile(url, formData, verificationToken, callback, callbackError);
                } else if (result.isDenied) {
                    //Swal.fire('Changes are not saved', '', 'info')
                }
            })
        }
        else {
            pemkot.ajaxformFile(url, formData, verificationToken, callback, callbackError);
        }

    },
    ajaxPartialView: (url, sendData = null, callback = null, callbackError = null) => {
        $.ajax({
            type: "POST",
            url: url,
            cache: false,
            data: sendData,
            success: (msg) => {
                if (callback != null) {
                    callback(msg);
                }
                else {
                    swalfireSuccessWithRefresh(msg);
                }
            },
            error: () => {
                if (callbackError == null) {
                    window.location = '/Home/Error';
                }
                else {
                    callbackError();
                }
            }
        });
    },
    addChatMessageLeft: (user, message, time) => {
        // Create a new list item
        var listItem = document.createElement("li");
        listItem.className = "chat-list left";
        var firstTwoChars = user.substring(0, 2);
        let splitUSer = user.split('-');
        // Create the chat message structure
        listItem.innerHTML = `
            <div class="conversation-list">
                <div class="chat-avatar">
                    <div class="avatar-sm">
                        <div class="avatar-title rounded-circle bg-soft-info  text-info"  data-bs-toggle="tooltip" data-bs-placement="top" title="${user}">
                            ${splitUSer[1]}
                        </div>
                    </div>
                </div>
                <div class="user-chat-content">
                    <div class="ctext-wrap">
                        <div class="ctext-wrap-content">
                            <p class="mb-0 ctext-content">${message}</p>
                        </div>
                    </div>
                    <div class="conversation-name">
                        <small class="text-muted time">${time}</small>
                        <span class="text-success check-message-icon">
                            <i class="ri-check-double-line align-bottom"></i>
                        </span>
                    </div>
                </div>
            </div>
        `;

        // Append the new chat message to the container
        document.getElementById("users-conversation").appendChild(listItem);
    },
    addChatMessageRight: (user, message, time) => {
        // Create a new list item
        var listItem = document.createElement("li");
        listItem.className = "chat-list right";
        var firstTwoChars = user.substring(0, 2);
        // Create the chat message structure
        listItem.innerHTML = `
            <div class="conversation-list">
                <div class="user-chat-content">
                    <div class="ctext-wrap">
                        <div class="ctext-wrap-content">
                            <p class="mb-0 ctext-content">${message}</p>
                        </div>
                    </div>
                    <div class="conversation-name">
                        <small class="text-muted time">${time}</small>
                        <span class="text-success check-message-icon">
                            <i class="ri-check-double-line align-bottom"></i>
                        </span>
                    </div>
                </div>
            </div>
        `;

        // Append the new chat message to the container
        document.getElementById("users-conversation").appendChild(listItem);
    },
    getTimeNow: () => {
        let now = new Date();

        // Get the current hour, minute, and second
        var hours = now.getHours();
        var minutes = now.getMinutes();
        var seconds = now.getSeconds();

        // Ensure two-digit formatting for hours, minutes, and seconds
        if (hours < 10) hours = "0" + hours;
        if (minutes < 10) minutes = "0" + minutes;
        if (seconds < 10) seconds = "0" + seconds;

        // Create a time string in 24-hour format (HH:MM:SS)
        var time24 = hours + ":" + minutes;

        return time24;
    }
}
function clone(obj) {
    if (null == obj || "object" != typeof obj) return obj;
    var copy = obj.constructor();
    for (var attr in obj) {
        if (obj.hasOwnProperty(attr)) {
            copy[attr] = obj[attr];
        }
    }
    return copy;
}