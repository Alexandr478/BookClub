const registerform = () => {

    let searchForm = document.forms["register-form"];

    if (typeof searchForm !== 'undefined') {
        searchForm.addEventListener('submit', function (event) {
            event.preventDefault();

            let registerFormContainer = document.querySelector(".register-error-container");
            let inputEmail = document.getElementById("RegisterInputEmail");
            let inputPassword = document.getElementById("RegisterInputPassword");
            let inputPasswordRetry = document.getElementById("RegisterInputPasswordRetry");


            var dataObject = {
                Email: inputEmail.value,
                Password: inputPassword.value,
                PasswordConfirm: inputPasswordRetry.value,
            }

            updateregisterForm(dataObject, registerFormContainer);
        });
    };
   
  
    async function updateregisterForm(dataObject, registerFormContainer) {

        let response = await fetch('Account/register/', {
            referrer: "origin",
            method: 'POST',
            credentials: 'include',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(dataObject)
        });


        if (response.ok) {
            let json = await response.json();

            if (json.registrationIsCorrect == false) {
                registerFormContainer.innerHTML = json.viewComponent;
            }
            else
            {

                window.location.href = window.location.origin;
              
            }
        }
    }
}

export default registerform;