async function login(email, password) {
    const request = await fetch(`https://localhost:7020/api/users/login`, {
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        cache: 'no-cache',
        method: 'POST',
        body: JSON.stringify({ 'email': email, 'password': password })
    });
    const data = await request.json();
    return data;
}

async function register(username, email, password) {
    const request = await fetch(`https://localhost:7020/api/users/register`, {
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        cache: 'no-cache',
        method: 'POST',
        body: JSON.stringify({ 'username': username, 'email': email, 'password': password })
    });
    const data = await request.json();
    return data;
}