window.initializeMap = (lat, lng) => {
    const container = document.getElementById('map');

    if (!container) {
        console.error("Map container not found.");
        return;
    }

    if (window.map) {
        window.map.remove();
    }

    window.map = L.map('map').setView([lat, lng], 13);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; OpenStreetMap contributors'
    }).addTo(window.map);

    L.marker([lat, lng]).addTo(window.map)
        .bindPopup('Você está aqui.')
        .openPopup();
};
