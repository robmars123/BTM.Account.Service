document.addEventListener('DOMContentLoaded', function () {
    var form = document.getElementById('registerForm');
    if (form) {
        form.addEventListener('submit', function () {
            var submitButton = document.getElementById('submitButton');
            if (submitButton) {
                submitButton.disabled = true;
                submitButton.innerHTML = "Submitting...";
            }
        });
    }
});