const bookSetDel = () => {

    let buttons = document.querySelectorAll(".btn-confirm-del")

    if (typeof buttons !== 'undefined') {
        buttons.forEach(function (item, index, array) {
            item.addEventListener("click", handlerConfirmation);
        });   
    }

    //обработчик нажатия на кнопку 
    function handlerConfirmation(event) {

        let dataObject = {
            Value: event.target.id,
        };

        updateBooks(dataObject);
    };

    async function updateBooks(dataObject) {

        let response = await fetch('Books/DelBookFromReadList/', {
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

            if (json.delRow) {
                {

                    let alert = document.querySelector(".alert");
                    if (alert.classList.contains("alert-danger"))
                    {
                        alert.classList.remove("alert-danger");
                        alert.classList.add("alert-success");
                    };
                    alert.innerHTML = json.message;  

                    let bookRow = document.getElementById("row "+json.bookId);
                    bookRow.remove();
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

export default bookSetDel;