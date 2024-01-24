function signOut() {
    localStorage.removeItem("jwt-token");
    window.location.href = '../login';
}

async function getUserId() {

    const token = localStorage.getItem("jwt-token");
    const currentUser = await getCurrentUser(token);

    return currentUser.id;
   
}



