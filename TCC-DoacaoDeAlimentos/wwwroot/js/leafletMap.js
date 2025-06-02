function initializeMap(lat, lng) {
    try {
        const container = document.getElementById('map');
        if (!container) {
            throw new Error("Map container not found");
        }

        if (typeof L === 'undefined') {
            throw new Error("Leaflet library not loaded");
        }

        container.style.display = 'block';
        container.innerHTML = "";

        const options = { preferCanvas: true };

        // 🟢 Salva o mapa no escopo global
        const map = L.map('map', options).setView([lat, lng], 13);
        window.leafletMap = map;

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            maxZoom: 19,
            attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
        }).addTo(map);

        L.marker([lat, lng]).addTo(map)
            .bindPopup('Você está aqui.')
            .openPopup();

        setTimeout(() => map.invalidateSize(), 100);

    } catch (error) {
        console.error("Map initialization error:", error);
        throw error;
    }
}
window.addMarkers = function (ongs) {
    if (!window.leafletMap) {
        console.error("Mapa não inicializado.");
        return;
    }

    if (window.ongMarkers) {
        window.ongMarkers.forEach(m => window.leafletMap.removeLayer(m));
    }
    window.ongMarkers = [];

    if (!ongs || !Array.isArray(ongs)) return;

    ongs.forEach(ong => {
        if (ong.latitude && ong.longitude) {
            const marker = L.marker([ong.latitude, ong.longitude])
                .addTo(window.leafletMap)
                .bindPopup(`<b>${ong.razaoSocial || ong.nome || "ONG"}</b><br>${ong.endereco || ""}`);
            window.ongMarkers.push(marker);
        }
    });
};
