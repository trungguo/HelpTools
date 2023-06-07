//TOAST
//Show toast Param explaination
//Title: You title
//Message: You message
//Type: success|info|error|warning
//Position: toast-top-right/left | toast-bottom-right/left | toast-top/bottom-full-width 
//Show/Hide Easing: swing|linear
//Show/Hide Method: show/hide|fadeIn/fadeOut|slideUp/slideDown
//Timing by "ms"
var MessageUtil = new function MessageUtil() {
    var self = this;
    self.showMessage = function (args) {
        toastr.options = {
            "closeButton": args.closeButton ?? true,
            "debug": args.debug ?? false,
            "newestOnTop": args.newestOnTop ?? true,
            "progressBar": args.progessBar ?? false,
            "positionClass": args.position ?? "toast-top-right",
            "preventDuplicates": args.preventDuplicates ?? true,
            "showDuration": args.showDuration ?? 200,
            "hideDuration": args.hideDuration ?? 800,
            "timeOut": (args.timeOut !== undefined && args.timeOut !== null) ? args.timeOut.toString() : "5000",
            "extendedTimeOut": (args.extenedTimeOut !== undefined && args.extenedTimeOut !== null) ? args.extenedTimeOut.toString() : "1000",
            "showEasing": args.showEasing ?? "swing",
            "hideEasing": args.hideEasing ?? "linear",
            "showMethod": args.showMethod ?? "fadeIn",
            "hideMethod": args.hideMethod ?? "fadeOut"
        }
        args.type = args.type ?? "success";
        args.message = args.message ?? "";
        args.title = args.title ?? "";
        switch (args.type) {
            case ("success"): {
                toastr.success(args.message, args.title);
            } break;
            case ("info"): {
                toastr.info(args.message, args.title);
            } break;
            case ("error"): {
                toastr.error(args.message, args.title);
            } break;
            case ("warning"): {
                toastr.warning(args.message, args.title);
            } break;
            default: {
                toastr.info(args.message, args.title);
            } break;
        }
    }
}
var DialogUtil = new function DialogUtil() {
    var self = this;
    //SWEET ALERT
    //Show dialog Param explaination
    //Title: You title
    //Message: You message
    //Icon: success|info|error|warning|question
    //CloseTime: 2000 (Auto enable progress bar)
    //ConfirmButton/Cancel: include Text, ArialLabel, CustomClass
    //show/hide AnimateClass can be found at https://animate.style
    //Image: Url, Height, Width, Alt
    //Background: Background css, background of dialog card
    //Backdrop: Backgroud fadeIn while dialog is shown
    //You can find more Example at: https://sweetalert2.github.io/
    self.showDialog = function (args) {
        var dialog = Swal.mixin({
            //Title - Icon - Footer
            html: args.message ?? null,
            title: args.title ?? "Notification",
            footer: args.footer ?? null,
            icon: args.icon ?? "info",
            //Image
            imageUrl: args.imageUrl ?? null,
            imageAlt: args.imageAlt ?? null,
            imageHeight: args.imageHeight ?? null,
            imageWidth: args.imageWidth ?? null,
            //Confirm button
            showConfirmButton: (args.confirmButtonText != null && args.confirmButtonText.length > 0) ? true : (args.cancelButtonText != null && args.cancelButtonText.length > 0) ? false : true,
            confirmButtonText: (args.confirmButtonText != null && args.confirmButtonText.length > 0) ? args.confirmButtonText : "OK",
            confirmButtonAriaLabel: args.confirmButtonAriaLabel ?? null,
            //Cancel button
            showCancelButton: (args.cancelButtonText != null && args.cancelButtonText.length > 0) ? true : false,
            cancelButtonText: args.cancelButtonText ?? null,
            cancelButtonAriaLabel: args.cancelButtonAriaLabel ?? null,
            customClass: {
                cancelButton: args.customCancelButtonClass ?? null,
                confirmButton: args.customConfirmButtonClass ?? null
            },
            // buttonsStyling: ((customCancelButtonClass != null && customCancelButtonClass.length != 0) || (customConfirmButtonClass != null && customConfirmButtonClass.length != 0)) ? false : true,
            //Dialog styling
            background: args.background ?? null,
            backdrop: (args.backdrop != null && args.backdrop.length > 0) ? args.backdrop : "rgba(0,0,0,.4)",
            padding: args.padding ?? null,
            timer: args.closeTime ?? null,
            timerProgressBar: (args.closeTime != null && args.closeTime > 0) ? true : false,
        });
        if (args.showAnimateClass != null && args.showAnimateClass.length != 0 && args.hideAnimateClass != null && args.hideAnimateClass.length != 0) {
            dialog.fire({
                showClass: { popup: args.showAnimateClass ?? null },
                hideClass: { popup: args.hideAnimateClass ?? null }
            }).then((result) => {
                return result
            });
        } else if (args.showAnimateClass != null && args.showAnimateClass.length != 0) {
            dialog.fire({
                showClass: { popup: args.showAnimateClass }
            }).then((result) => {
                return result
            });
        }
        else if (args.hideAnimateClass != null && args.hideAnimateClass.length != 0) {
            dialog.fire({
                hideClass: { popup: args.hideAnimateClass }
            }).then((result) => {
                return result
            });
        }
        else {
            dialog.fire().then((result) => {
                return result
            });
        }
    }
}