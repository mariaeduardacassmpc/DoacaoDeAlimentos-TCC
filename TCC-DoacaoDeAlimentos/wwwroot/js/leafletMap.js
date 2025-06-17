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

    const greenIcon = new L.Icon({
        iconUrl: 'https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-green.png',
        shadowUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png',
        iconSize: [25, 41],
        iconAnchor: [12, 41],
        popupAnchor: [1, -34],
        shadowSize: [41, 41]
    });


    for (const [index, ong] of ongs.entries()) {
        if (typeof ong.latitude !== "number" || typeof ong.longitude !== "number" || ong.latitude === 0 || ong.longitude === 0) {
            continue;
        }

        try {
            const marker = L.marker([ong.latitude, ong.longitude], {
                title: ong.nomeFantasia,
                icon: greenIcon 
            }).addTo(window.leafletMap);

            const popupHtml = `
            <div style="
                background: #fff;
                color: #333;
                border-radius: 10px;
                padding: 10px 12px;
                box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
                max-width: 220px;
                font-size: 0.85rem;
            ">
                <div style="font-weight: 700; margin-bottom: 6px; color: #28a745; font-size: 0.95rem;">
                    ${ong.nomeFantasia}
                </div>
                <div style="line-height: 1.4;">
                    <i class="fas fa-map-marker-alt" style="color:#28a745; margin-right:4px;"></i> ${ong.endereco || 'Não informado'}<br>
                    <i class="fas fa-phone" style="color:#28a745; margin-right:4px;"></i> ${ong.telefone || 'Não informado'}<br>
                    <i class="fas fa-user" style="color:#28a745; margin-right:4px;"></i> ${ong.responsavel || 'Não informado'}
                </div>
                <div style="text-align: center; margin-top: 10px;">
                    <button 
                        onclick="window.location.href='/CadastroDeDoacao/${ong.id}'" 
                        style="
                            background: linear-gradient(90deg, #28a745 0%, #218838 100%);
                            color: white;
                            border: none;
                            border-radius: 20px;
                            padding: 6px 16px;
                            font-weight: 600;
                            font-size: 0.85rem;
                            box-shadow: 0 2px 6px rgba(40, 167, 69, 0.3);
                            cursor: pointer;
                            letter-spacing: 0.5px;
                            transition: all 0.2s ease;
                        "
                        onmouseover="this.style.background='linear-gradient(90deg, #218838 0%, #28a745 100%)'; this.style.transform='scale(1.05)'; this.style.boxShadow='0 4px 12px rgba(40, 167, 69, 0.4)'"
                        onmouseout="this.style.background='linear-gradient(90deg, #28a745 0%, #218838 100%)'; this.style.transform='scale(1)'; this.style.boxShadow='0 2px 6px rgba(40, 167, 69, 0.3)'"
                    >
                        Doar
                    </button>
                </div>
            </div>
        `;
            marker.bindPopup(popupHtml, {
                className: 'popup-ong'
            });

        } catch (error) {
            continue;
        }
    }
};
ache
