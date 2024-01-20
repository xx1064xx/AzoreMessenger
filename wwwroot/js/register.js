async function submitRegisterForm(e) {
    e.preventDefault();

    const username = document.querySelector('#username').value;
    const email = document.querySelector('#email').value;
    const password = document.querySelector('#password').value;

    try {
        const loginInfo = await register(firstName, familyName, userName, email, password);
        localStorage.setItem('jwt-token', loginInfo.jwt);
        window.location.href = '../';
    }
    catch (err) {
        document.querySelector('#register-error').innerText = err.message;
    }


}

document.querySelector('form').addEventListener('submit', submitRegisterForm);