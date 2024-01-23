async function submitRegisterForm(e) {
    e.preventDefault();

    const email = document.querySelector('#email').value;
    const password = document.querySelector('#password').value;
    var passInput = document.getElementById('password');

    try {
        const loginInfo = await login(email, password);
        if (loginInfo.jwt !== undefined) {
            localStorage.setItem('jwt-token', loginInfo.jwt);
            window.location.href = '../index';
        }
        else {
            passInput.style.border = '2px solid red';
            passInput.placeholder = 'Passwort oder Email ist falsch';
            passInput.value = '';
        }
    }
    catch (err) {
        
    }

}

document.addEventListener('DOMContentLoaded', (event) => {
    document.querySelector('form').addEventListener('submit', submitRegisterForm);
});
