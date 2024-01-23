async function submitRegisterForm(e) {
    e.preventDefault();

    const email = document.querySelector('#email').value;
    const password = document.querySelector('#password').value;

    try {
        const loginInfo = await login(email, password);
        if (loginInfo.jwt !== undefined) {
            localStorage.setItem('jwt-token', loginInfo.jwt);
            window.location.href = '../index';
        }
        else {
            console.log("Login ERROR")
        }
    }
    catch (err) {
        console.log("Login ERROR")
    }

    console.log("Email: " + email);
    console.log("password: " + password);
    console.log("Login Info: " + loginInfo);

}

document.addEventListener('DOMContentLoaded', (event) => {
    document.querySelector('form').addEventListener('submit', submitRegisterForm);
});
