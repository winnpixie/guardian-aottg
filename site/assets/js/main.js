(function () {
    window.onBodyLoad = () => {
        window.fetch('version.txt', {
            cache: 'no-store'
        }).then(response => {
            if (!response.ok) return;

            response.text().then(txt => {
                let infoText = document.getElementById('info-text');
                if (!infoText) return; // How.

                infoText.innerText = infoText.innerText.replace('$BUILD$', txt);
            });
        });
    }
})();