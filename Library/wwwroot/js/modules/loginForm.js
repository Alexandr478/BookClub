const loginform = () => {

    let searchForm = document.forms["login-form"];

    if (typeof searchForm !== 'undefined')
    {
        searchForm.addEventListener('submit', function (event) {
            event.preventDefault();

            let token = gettoken();
            let loginFormContiner = document.querySelector(".login-error-container");
            let inputEmail = document.getElementById("loginInputEmail");
            let inputPassword = document.getElementById("loginInputPassword");


            var dataObject = {
                Email: inputEmail.value,
                Password: inputPassword.value,
                RememberMe: true
            }

            updateLoginForm(dataObject, token, loginFormContiner);
        });
    };
   

    
    async function updateLoginForm(dataObject, token, loginFormContiner) {

        let response = await fetch('Account/Login/', {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            },
            body: JSON.stringify(dataObject)
        });


        if (response.ok) {
            let json = await response.json();

            if (json.loginIsCorrect == false) {
                loginFormContiner.innerHTML = json.viewComponent;
            }
            else
            {
      
                window.location.href = window.location.origin;
            }
        }
    }
}

export default loginform;