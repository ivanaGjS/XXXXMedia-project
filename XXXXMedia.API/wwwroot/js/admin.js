(async () => {
    const getJwt = () => localStorage.getItem('jwt');

    if (!getJwt()) {
        location.href = '/';
        return;
    }

    const loadBtn = document.getElementById('loadClients');
    const output = document.getElementById('out');

    loadBtn.addEventListener('click', async () => {
        try {
            const res = await fetch('/api/clients', {
                headers: { Authorization: `Bearer ${getJwt()}` }
            });

            if (!res.ok) {
                output.innerText = `Error: ${res.status}`;
                if (res.status === 401) location.href = '/';
                return;
            }

            const data = await res.json();
            output.innerText = JSON.stringify(data, null, 2);
        } catch (err) {
            console.error("Error loading clients:", err);
            output.innerText = "Error loading clients.";
        }
    });
})();
