function signOut() {
    localStorage.removeItem("jwt-token");
    toggleProfile();
}