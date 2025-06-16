window.getUserLocation = () => {
    return new Promise((resolve, reject) => {
        // Verifica se o navegador suporta a API de Geolocalização
        if (!navigator.geolocation) {
            reject('Geolocalização não é suportada');
            return; 
        }

        // Tenta obter a localização atual do usuário
        navigator.geolocation.getCurrentPosition(
            (position) => {
                const data = {
                    lat: position.coords.latitude,  
                    lng: position.coords.longitude 
                };
                resolve(data);
            },
            (error) => {
                reject(error.message);
            }
        );
    });
};