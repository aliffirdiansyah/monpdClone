if(document.querySelectorAll("[toast-list]") || document.querySelectorAll('[data-choices]') || document.querySelectorAll("[data-provider]")){
    var scripttoastify = document.createElement('script');
    scripttoastify.src="../assets/libs/toastify-js/toastify.js";
    document.head.appendChild(scripttoastify);
    var scriptchoices = document.createElement('script');
    scriptchoices.src ="../assets/libs/choices.js/public/assets/scripts/choices.min.js";
    document.head.appendChild(scriptchoices); 
    var scriptflatpickr = document.createElement('script');
    scriptflatpickr.src="../assets/libs/flatpickr/flatpickr.min.js";
    document.head.appendChild(scriptflatpickr);   
  }