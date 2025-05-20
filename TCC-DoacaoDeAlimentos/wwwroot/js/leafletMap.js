function initializeMap(lat, lng) {
    try
    {
        const container = document.getElementById('map');
        if (!container) {
            throw new Error("Map container not found");
        }

        if (typeof L === 'undefined') {
            throw new Error("Leaflet library not loaded");
        }

        // Garante que o container esteja visível
        container.style.display = 'block';

        // Limpa o conteúdo anterior do mapa (se precisar reinicializar)
        container.innerHTML = "";

        const options = {
            preferCanvas: true 
        };

        const map = L.map('map', options).setView([lat, lng], 13);

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
