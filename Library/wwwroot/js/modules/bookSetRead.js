const bookSetRead = () => {

    let buttons = document.querySelectorAll(".btn-confirm-read")

    if (typeof buttons !== 'undefined') {
        buttons.forEach(function (item, index, array) {
            item.addEventListener("click", handlerConfirmation);
        });
    }


    buttons.forEach(function (item, index, array) {
        item.addEventListener("click", handlerConfirmation);
    });    

    //обработчик нажатия на кнопку 
    function handlerConfirmation(event) {

        let dataObject = {
            Value: event.target.id,
        };

        updateBooks(dataObject);
    };

    async function updateBooks(dataObject) {

        let response = await fetch('Books/AddBookToReadList/', {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(dataObject)
        });


        if (response.ok) {
            let json = await response.json();

            if (json.changeButtonStatus) {
                {
                    let btn = document.getElementById(json.bookId);
                    btn.classList.remove('btn-warning','btn-confirm-read');
                    btn.classList.add('btn-success');
                    btn.disabled = true;
                    btn.innerHTML = "Прочитано"

                    let alert = document.querySelector(".alert");
                    if (alert.classList.contains("alert-danger"))
                    {
                        alert.classList.remove("alert-danger");
                        alert.classList.add("alert-success");
                    };

                    alert.innerHTML = json.message;

                    let readerCount = document.getElementById("readerCount "+json.bookId);
                    readerCount.innerHTML = json.userReadCount;
                }            
            }
            else
            {
                let alert = document.querySelector(".alert");
                if (alert.classList.contains("alert-success")) {
                    alert.classList.remove("alert-danger");
                    alert.classList.add("alert-danger");
                };

                alert.innerHTML = json.message;               
            }
        }
    }
};

export default bookSetRead;