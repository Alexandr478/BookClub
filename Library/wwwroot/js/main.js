
import registerForm from './modules/registerForm.js';
import loginForm    from './modules/loginForm.js';
import bookSetRead from './modules/bookSetRead.js';
import bookSetDel from './modules/bookSetDel.js';

window.addEventListener('DOMContentLoaded', () => {
    "use strict";

    loginForm();
    registerForm();
    bookSetRead(); 
    bookSetDel(); 
});
















