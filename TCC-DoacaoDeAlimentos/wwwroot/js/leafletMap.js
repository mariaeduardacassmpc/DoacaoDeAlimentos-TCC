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
        console.error("Mapa não inicializado");
        return;
    }

    window.leafletMap.eachLayer(function (layer) {
        if (layer instanceof L.Marker && !layer.getPopup().getContent().includes("Você está aqui.")) {
            window.leafletMap.removeLayer(layer);
        }
    });

    // Use for...of que é mais amigável para depuração
    for (const [index, ong] of ongs.entries()) {
        console.log(`Processando ONG ${index + 1}/${ongs.length}`, ong);

        try {
            // Crie o marcador
            const marker = L.marker([ong.latitude, ong.longitude], {
                title: ong.nome
            }).addTo(map);

            console.log(`Marcador criado para ${ong.nome}`, marker);

            // Adicione o popup
            marker.bindPopup(`<b>${ong.nome}</b><br>${ong.endereco}`);
            console.log(`Popup adicionado para ${ong.nome}`);

        } catch (error) {
            console.error(`Erro na ONG ${ong.nome}:`, error);
            continue; // Pula para a próxima ONG se houver erro
        }
    }
};