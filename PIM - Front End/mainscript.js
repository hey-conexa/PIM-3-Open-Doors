window.addEventListener('load', () => {
    
    // CAROUSEL APOIADORES
    document.querySelectorAll('.carousel-imagens').forEach(track => {
        const velocidade = 1;

        const wrapper = track.parentElement;
        const larguraWrapper = wrapper.offsetWidth;
        const imagensOriginais = Array.from(track.querySelectorAll('img'));
        
        while (track.scrollWidth < larguraWrapper * 3) {
            imagensOriginais.forEach(img => {
                track.appendChild(img.cloneNode(true));
            });
        }

        const larguraItem = imagensOriginais[0].offsetWidth + 20;
        let posicao = 0;

        function moverCarousel() {
            posicao -= velocidade;

            const primeiraImagem = track.querySelector('img');

            if (Math.abs(posicao) >= larguraItem) {
                track.appendChild(primeiraImagem);
                posicao += larguraItem;
            }

            track.style.transform = `translateX(${posicao}px)`;
            requestAnimationFrame(moverCarousel);
        }

        requestAnimationFrame(moverCarousel);
    });


    // MURAL
    (function () {
        const track  = document.getElementById('muralTrack');
        const dots   = document.querySelectorAll('.mural-dot');
        const thumbs = document.querySelectorAll('.mural-thumb');

        if (!track) return;

        let current = 0;
        const total = dots.length;
        let timer;

        function muralGoTo(n) {
            current = (n + total) % total;
            track.style.transform = `translateX(-${current * 100}%)`;
            dots.forEach((d, i)  => d.classList.toggle('mural-dot--active',   i === current));
            thumbs.forEach((t, i) => t.classList.toggle('mural-thumb--active', i === current));
            clearInterval(timer);
            timer = setInterval(() => muralGoTo(current + 1), 4000);
        }

        window.muralGoTo = muralGoTo;
        window.muralMove = function (dir) { muralGoTo(current + dir); };

        timer = setInterval(() => muralGoTo(current + 1), 4000);
    })();

});