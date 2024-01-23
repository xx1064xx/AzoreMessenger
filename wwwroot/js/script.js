function signOut() {
    localStorage.removeItem("jwt-token");
    window.location.href = '../login';
}

