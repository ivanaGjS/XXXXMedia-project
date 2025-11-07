document.getElementById("login").addEventListener("click", async () => {
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;

    const res = await fetch('/api/auth/login', {
        method: 'POST', headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ Username: username, Password: password })
    });

    const data = await res.json();
    if (res.ok && data.token) {
        localStorage.setItem('jwt', data.token);
        const role = document.getElementById("role").value;
        if (role === 'Admin') location.href = '/admin.html';
        else location.href = '/client.html';
    } else {
        document.getElementById("msg").innerText = data.message || "Login failed";
    }
});
