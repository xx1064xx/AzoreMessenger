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

async function getCurrentUser(token) {
    const request = await fetch(`https://localhost:7020/api/users/getCurrentUser`, {
        headers: {
            'Accept': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        method: 'GET'
    });
    const data = await request.json();
    return data;
}

async function setBrowser(userId, browsername) {
    const request = await fetch(`https://localhost:7020/api/browsers/setBrowser`, {
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        cache: 'no-cache',
        method: 'POST',
        body: JSON.stringify({ 'userId': userId, 'browsername': browsername})
    });
    const data = await request.json();
    return data;
}