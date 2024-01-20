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
            document.querySelector('#login-error').innerText = 'Login Failed.';
        }
    }
    catch (err) {
        document.querySelector('#login-error').innerText = err.message;
    }

    console.log("Email: " + email);
    console.log("password: " + password);
    console.log("Login Info: " + loginInfo);

}

document.querySelector('#form').addEventListener('submit', submitRegisterForm);