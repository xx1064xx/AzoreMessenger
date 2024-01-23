async function submitRegisterForm(e) {
    e.preventDefault();

    const username = document.querySelector('#username').value;
    const email = document.querySelector('#email').value;
    const password = document.querySelector('#password').value;
    const confirmPassword = document.querySelector('#confirmPassword').value;

    var confirmPassInput = document.getElementById('confirmPassword');

    if (password == confirmPassword) {

        try {
            const loginInfo = await register(username, email, password);
            localStorage.setItem('jwt-token', loginInfo.jwt);
            window.location.href = '../';
        }
        catch (err) {
            console.log("login ERROR")
        }
    }
    else {

        confirmPassInput.style.border = '2px solid red';
        confirmPassInput.placeholder = 'Passwort muss gleich sein';
        confirmPassInput.value = '';

    }

    
}

document.addEventListener('DOMContentLoaded', (event) => {
    document.querySelector('form').addEventListener('submit', submitRegisterForm);
});
