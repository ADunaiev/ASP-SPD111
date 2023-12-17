﻿document.addEventListener('DOMContentLoaded', () => {
    const authButton = document.getElementById("auth-button");
    if (authButton) authButton.addEventListener('click', authButtonClick);

    const signOut = document.getElementById("auth-signout-button");
    if (signOut) {
        signOut.addEventListener('click', signOutButtonClick);
    }

    const saveProfileButton = document.getElementById("profile-save-button");
    if (saveProfileButton) {
        saveProfileButton.addEventListener('click', saveProfileButtonClick);
    } 

    const deleteProfileButton = document.getElementById("profile-delete-button");
    if (deleteProfileButton) {
        deleteProfileButton.addEventListener('click', deleteProfileButtonClick);
    } 
    
   
});

function authButtonClick() {
    const loginInput = document.getElementById("auth-login");
    if (!loginInput) throw "Element #auth-login not found";
    const login = loginInput.value.trim();
    if (login.length == 0) {
        showAuthMessage("Заповнить логін!");
        return;
    }

    const passwordInput = document.getElementById("auth-password");
    if (!passwordInput) throw "Element #auth-password not found";
    const password = passwordInput.value.trim();
    if (password.length == 0) {
        showAuthMessage("Заповнить пароль!");
        return;
    }
    
    fetch(`/api/auth?login=${login}&password=${password}`)
        .then(r => {
            if (r.status == 200) { // OK
                window.location.reload();             
            }
            else {  // 401
                showAuthMessage("Вхід відхилено");
            }
        });
}

function signOutButtonClick() {

    return fetch(`/api/auth`, { method: 'delete' })
        .then(r => {
            if (r.status == 200) {
                window.location.href = `/home/index`;
            }
            else {
                showAuthMessage("Виникли проблеми з сервером");
            }
        });
    
}

function showAuthMessage(message) {
    const authMessage = document.getElementById("auth-message");
    if (!authMessage) throw "Element #auth - message not found";

    authMessage.innerText = message;
    authMessage.classList.remove("visually-hidden");
}

function saveProfileButtonClick() {

    const nameInput = document.querySelector('input[name="profile-name"]');
    if (!nameInput) throw "Element input[name ='profile-name'] not found";
    

    const emailInput = document.querySelector('input[name="profile-email"]');
    if (!emailInput) throw "Element input[name ='profile-email'] not found";

    fetch(`/user/UpdateProfile?newName=${nameInput.value}&newEmail=${emailInput.value}`,
        {
            method: 'POST'
        })
        .then(r => r.json())
        .then(j => {
            console.log(j);
        });

}

function deleteProfileButtonClick() {
    fetch(`/user/DeleteProfile`,
        {
            method: 'DELETE'
        })
        .then(r => r.json())
        .then(j => {
            console.log(j);
        });
        
}