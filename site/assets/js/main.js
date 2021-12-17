(function () {
    window.onBodyLoad = () => {
        window.fetch('version_data.json', {
            cache: 'no-store'
        }).then(response => {
            if (!response.ok) return;

            response.json().then(jsonObj => {
                let infoText = document.getElementById('info-text');
                if (!infoText) return; // How.

                infoText.innerText = infoText.innerText
                    .replace('$BUILD$', jsonObj.current_build)
                    .replace('$REQUIRED$', jsonObj.required ? 'required' : 'recommended, but not necessary');
            });
        });
    }
})();