(async () => {
    const token = localStorage.getItem('jwt');
    if (!token) {
        location.href = '/';
        return;
    }

    try {
        const meRes = await fetch('/api/auth/me', {
            headers: { Authorization: `Bearer ${token}` }
        });

        if (!meRes.ok) {
            console.error("Failed to fetch user data", meRes.status);
            localStorage.removeItem('jwt');
            location.href = '/';
            return;
        }

        const me = await meRes.json();
        document.getElementById('out').innerText = JSON.stringify(me, null, 2);
    } catch (err) {
        console.error("Error loading client data", err);
    }
})();
