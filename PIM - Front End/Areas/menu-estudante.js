/* Lógica da Área do Estudante e Teste Vocacional */


document.addEventListener('DOMContentLoaded', () => {
    try {
        const raw     = localStorage.getItem('od-session');
        const session = raw ? JSON.parse(raw) : null;

        if (!session || session.type !== 'student') {

            window.location.href = '../Acesso/AcessoEstudantes.html';
            return;
        }

        const name    = session.name || 'Estudante';
        const initial = name.charAt(0).toUpperCase();

        document.getElementById('heroName').textContent   = `Olá, ${name.split(' ')[0]}!`;
        document.getElementById('heroAvatar').textContent = initial;


        const pct = session.photo ? 80 : 40;
        document.getElementById('progressPct').textContent  = pct + '%';
        document.getElementById('progressFill').style.width = pct + '%';
        document.querySelector('[role="progressbar"]').setAttribute('aria-valuenow', pct);
        document.querySelector('[role="progressbar"]').setAttribute('aria-label', `${pct}% do perfil completo`);

    } catch (e) {
        console.warn('Erro ao carregar sessão:', e);
    }
});

function doLogout() {
    try { localStorage.removeItem('od-session'); } catch (e) {}
    window.location.href = '../OpenDoors.html';
}


// SEÇÃO TESTE VOCACIONAL 

const VOC_QUESTIONS = [

    {
        type: 'choice',
        typeLabel: 'Escolha uma opção',
        text: 'Qual dessas atividades você mais gostaria de fazer no trabalho?',
        options: [
            { label: 'Criar e desenvolver sistemas ou programas',       areas: ['TI', 'Engenharia'] },
            { label: 'Cuidar e ajudar pessoas diretamente',             areas: ['Saúde', 'Educação'] },
            { label: 'Organizar processos, finanças e dados',           areas: ['Administração', 'Finanças'] },
            { label: 'Criar conteúdo, design ou comunicação',           areas: ['Comunicação', 'Design'] },
            { label: 'Construir, projetar ou trabalhar com materiais',  areas: ['Engenharia', 'Indústria'] },
        ]
    },
    {
        type: 'choice',
        typeLabel: 'Escolha uma opção',
        text: 'Como você prefere trabalhar?',
        options: [
            { label: 'Sozinho, com autonomia e foco',               areas: ['TI', 'Análise de Dados'] },
            { label: 'Em equipe, colaborando com outras pessoas',   areas: ['Administração', 'Comunicação'] },
            { label: 'Com o público, atendendo clientes',           areas: ['Vendas', 'Educação'] },
            { label: 'Em campo, em movimento',                      areas: ['Logística', 'Indústria'] },
            { label: 'Em laboratório ou ambiente técnico',          areas: ['Saúde', 'Engenharia'] },
        ]
    },
    {
        type: 'choice',
        typeLabel: 'Escolha uma opção',
        text: 'Qual matéria escolar você mais gostava (ou gosta)?',
        options: [
            { label: 'Matemática e Física',         areas: ['Engenharia', 'TI', 'Finanças'] },
            { label: 'Biologia e Química',          areas: ['Saúde', 'Indústria'] },
            { label: 'História e Geografia',        areas: ['Direito', 'Administração'] },
            { label: 'Português e Literatura',      areas: ['Comunicação', 'Educação'] },
            { label: 'Artes e Educação Física',     areas: ['Design', 'Esporte'] },
        ]
    },
    {
        type: 'choice',
        typeLabel: 'Escolha uma opção',
        text: 'O que mais te motiva em um trabalho?',
        options: [
            { label: 'Resolver problemas complexos',             areas: ['TI', 'Engenharia', 'Análise de Dados'] },
            { label: 'Ajudar e impactar a vida das pessoas',    areas: ['Saúde', 'Educação', 'Serviço Social'] },
            { label: 'Crescimento financeiro e estabilidade',   areas: ['Finanças', 'Administração', 'Vendas'] },
            { label: 'Liberdade criativa e inovação',           areas: ['Design', 'Comunicação', 'Marketing'] },
            { label: 'Fazer parte de algo grande e importante', areas: ['Direito', 'Engenharia', 'Logística'] },
        ]
    },
    {
        type: 'choice',
        typeLabel: 'Escolha uma opção',
        text: 'Em um projeto em grupo, qual papel você costuma assumir?',
        options: [
            { label: 'Liderança — organizo e distribuo tarefas',            areas: ['Administração', 'Vendas'] },
            { label: 'Executor — prefiro fazer do que coordenar',            areas: ['TI', 'Indústria'] },
            { label: 'Criativo — trago ideias e soluções novas',            areas: ['Design', 'Marketing'] },
            { label: 'Analista — cuido dos dados e da qualidade',           areas: ['Finanças', 'Análise de Dados'] },
            { label: 'Comunicador — apresento e mediei conflitos',          areas: ['Comunicação', 'Direito'] },
        ]
    },
    {
        type: 'choice',
        typeLabel: 'Escolha uma opção',
        text: 'Qual desses ambientes de trabalho te atrai mais?',
        options: [
            { label: 'Escritório moderno com tecnologia',        areas: ['TI', 'Finanças', 'Administração'] },
            { label: 'Hospital, clínica ou laboratório',         areas: ['Saúde'] },
            { label: 'Agência criativa ou estúdio',              areas: ['Design', 'Comunicação', 'Marketing'] },
            { label: 'Fábrica, obra ou campo',                   areas: ['Indústria', 'Engenharia', 'Logística'] },
            { label: 'Escola ou espaço educativo',               areas: ['Educação'] },
        ]
    },
    {
        type: 'choice',
        typeLabel: 'Escolha uma opção',
        text: 'Como você prefere aprender algo novo?',
        options: [
            { label: 'Lendo documentações e tutoriais',          areas: ['TI', 'Análise de Dados'] },
            { label: 'Praticando e errando até acertar',         areas: ['Indústria', 'Engenharia'] },
            { label: 'Ouvindo e conversando com especialistas',  areas: ['Saúde', 'Educação', 'Direito'] },
            { label: 'Assistindo vídeos e exemplos visuais',     areas: ['Design', 'Comunicação'] },
            { label: 'Analisando dados e padrões',               areas: ['Finanças', 'Análise de Dados'] },
        ]
    },

    // ESCALAS

    {
        type: 'scale',
        typeLabel: 'Avalie sua afinidade (1 = nenhuma, 5 = muita)',
        text: 'Tenho facilidade com computadores, programação ou tecnologia.',
        area: 'TI',
    },
    {
        type: 'scale',
        typeLabel: 'Avalie sua afinidade (1 = nenhuma, 5 = muita)',
        text: 'Gosto de cuidar, ouvir e ajudar pessoas que precisam de apoio.',
        area: 'Saúde',
    },
    {
        type: 'scale',
        typeLabel: 'Avalie sua afinidade (1 = nenhuma, 5 = muita)',
        text: 'Me interesso por negócios, empreendedorismo e gestão.',
        area: 'Administração',
    },
    {
        type: 'scale',
        typeLabel: 'Avalie sua afinidade (1 = nenhuma, 5 = muita)',
        text: 'Tenho facilidade para me expressar, escrever ou falar em público.',
        area: 'Comunicação',
    },
    {
        type: 'scale',
        typeLabel: 'Avalie sua afinidade (1 = nenhuma, 5 = muita)',
        text: 'Gosto de cálculos, números e raciocínio lógico.',
        area: 'Finanças',
    },
    {
        type: 'scale',
        typeLabel: 'Avalie sua afinidade (1 = nenhuma, 5 = muita)',
        text: 'Me interesso por projetos de construção, mecânica ou elétrica.',
        area: 'Engenharia',
    },
    {
        type: 'scale',
        typeLabel: 'Avalie sua afinidade (1 = nenhuma, 5 = muita)',
        text: 'Tenho habilidade ou interesse em design, ilustração ou artes visuais.',
        area: 'Design',
    },
    {
        type: 'scale',
        typeLabel: 'Avalie sua afinidade (1 = nenhuma, 5 = muita)',
        text: 'Gosto de ensinar, explicar ou transmitir conhecimento para outros.',
        area: 'Educação',
    },
];

const VOC_AREAS = {
    'TI':               { icon: '💻', label: 'Tecnologia da Informação', vagas: ['Estágio em Desenvolvimento Web', 'Suporte de TI', 'Análise de Sistemas'] },
    'Saúde':            { icon: '🏥', label: 'Saúde',                     vagas: ['Auxiliar de Enfermagem', 'Estágio em Farmácia', 'Agente de Saúde'] },
    'Administração':    { icon: '📊', label: 'Administração',              vagas: ['Assistente Administrativo', 'Jovem Aprendiz — RH', 'Estágio em Gestão'] },
    'Finanças':         { icon: '💰', label: 'Finanças',                   vagas: ['Jovem Aprendiz — Financeiro', 'Auxiliar Contábil', 'Estágio em Controladoria'] },
    'Comunicação':      { icon: '📢', label: 'Comunicação e Marketing',    vagas: ['Estágio em Marketing Digital', 'Assistente de Comunicação', 'Produtor de Conteúdo'] },
    'Design':           { icon: '🎨', label: 'Design',                     vagas: ['Estágio em Design Gráfico', 'Auxiliar de Criação', 'Estágio em UX/UI'] },
    'Engenharia':       { icon: '⚙️', label: 'Engenharia',                 vagas: ['Estágio em Engenharia Civil', 'Auxiliar Técnico', 'Jovem Aprendiz — Indústria'] },
    'Educação':         { icon: '📚', label: 'Educação',                   vagas: ['Monitor de Ensino', 'Auxiliar de Creche', 'Estágio em Pedagogia'] },
    'Logística':        { icon: '🚚', label: 'Logística',                  vagas: ['Jovem Aprendiz — Logística', 'Auxiliar de Estoque', 'Assistente de Supply Chain'] },
    'Indústria':        { icon: '🏭', label: 'Indústria',                  vagas: ['Jovem Aprendiz — Produção', 'Operador de Máquinas', 'Auxiliar de Qualidade'] },
    'Análise de Dados': { icon: '📈', label: 'Análise de Dados',           vagas: ['Estágio em BI', 'Auxiliar de Análise', 'Jovem Aprendiz — TI'] },
    'Vendas':           { icon: '🤝', label: 'Vendas e Atendimento',       vagas: ['Jovem Aprendiz — Vendas', 'Atendente Comercial', 'Assistente de Vendas'] },
    'Direito':          { icon: '⚖️', label: 'Direito e Jurídico',         vagas: ['Estágio Jurídico', 'Assistente Paralegal', 'Jovem Aprendiz — Administrativo'] },
};

let vocCurrent   = 0;
let vocAnswers   = {};
let vocDone      = false;

function vocStart() {
    document.getElementById('voc-intro').style.display = 'none';
    document.getElementById('voc-test').style.display  = 'block';
    vocCurrent = 0;
    vocAnswers = {};
    vocRenderQuestion();
}

function vocRenderQuestion() {
    const q       = VOC_QUESTIONS[vocCurrent];
    const total   = VOC_QUESTIONS.length;
    const pct     = Math.round((vocCurrent / total) * 100);

    document.getElementById('vocQNum').textContent        = vocCurrent + 1;
    document.getElementById('vocProgressFill').style.width = pct + '%';
    document.getElementById('vocProgressBar').setAttribute('aria-valuenow', pct);

    document.getElementById('vocQType').textContent = q.typeLabel;
    document.getElementById('vocQText').textContent = q.text;

    document.getElementById('vocBtnBack').style.visibility = vocCurrent > 0 ? 'visible' : 'hidden';

    const btnNext = document.getElementById('vocBtnNext');
    const isLast  = vocCurrent === total - 1;
    btnNext.textContent = isLast ? 'Ver resultado' : 'Próxima →';
    btnNext.disabled    = !(vocCurrent in vocAnswers);

    const container = document.getElementById('vocOptions');
    container.innerHTML = '';

    if (q.type === 'choice') {
        container.className = 'voc-options voc-options--choice';
        q.options.forEach((opt, i) => {
            const btn = document.createElement('button');
            btn.className   = 'voc-opt-choice';
            btn.textContent = opt.label;
            btn.setAttribute('role', 'radio');
            btn.setAttribute('aria-checked', 'false');
            if (vocAnswers[vocCurrent]?.selectedIndex === i) {
                btn.classList.add('selected');
                btn.setAttribute('aria-checked', 'true');
            }
            btn.addEventListener('click', () => {
                container.querySelectorAll('.voc-opt-choice').forEach(b => {
                    b.classList.remove('selected');
                    b.setAttribute('aria-checked', 'false');
                });
                btn.classList.add('selected');
                btn.setAttribute('aria-checked', 'true');
                vocAnswers[vocCurrent] = { areas: opt.areas, selectedIndex: i };
                document.getElementById('vocBtnNext').disabled = false;
            });
            container.appendChild(btn);
        });

    } else if (q.type === 'scale') {
        container.className = 'voc-options voc-options--scale';
        const saved = vocAnswers[vocCurrent]?.value ?? null;

        const labels = ['1 Nenhuma', '2 Pouca', '3 Média', '4 Boa', '5 Muita'];
        labels.forEach((lbl, i) => {
            const val = i + 1;
            const btn = document.createElement('button');
            btn.className = 'voc-opt-scale';
            if (saved === val) btn.classList.add('selected');
            const [num, txt] = lbl.split('\n');
            btn.innerHTML = `<strong>${num}</strong><span>${txt}</span>`;
            btn.setAttribute('aria-label', `${val} — ${txt}`);
            btn.setAttribute('role', 'radio');
            btn.setAttribute('aria-checked', saved === val ? 'true' : 'false');
            btn.addEventListener('click', () => {
                container.querySelectorAll('.voc-opt-scale').forEach(b => {
                    b.classList.remove('selected');
                    b.setAttribute('aria-checked', 'false');
                });
                btn.classList.add('selected');
                btn.setAttribute('aria-checked', 'true');
                vocAnswers[vocCurrent] = { area: q.area, value: val };
                document.getElementById('vocBtnNext').disabled = false;
            });
            container.appendChild(btn);
        });
    }
}

function vocNext() {
    if (!(vocCurrent in vocAnswers)) return;
    if (vocCurrent === VOC_QUESTIONS.length - 1) {
        vocShowResult();
    } else {
        vocCurrent++;
        vocRenderQuestion();
        document.getElementById('vocQuestionWrap').scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    }
}

function vocBack() {
    if (vocCurrent > 0) {
        vocCurrent--;
        vocRenderQuestion();
    }
}

function vocShowResult() {

    const scores = {};

    Object.values(vocAnswers).forEach(ans => {
        if (ans.areas) {

            ans.areas.forEach(a => {
                scores[a] = (scores[a] || 0) + 2;
            });
        } else if (ans.area && ans.value) {

            scores[ans.area] = (scores[ans.area] || 0) + ans.value;
        }
    });


    const ranked = Object.entries(scores)
        .sort((a, b) => b[1] - a[1])
        .slice(0, 3)
        .map(([area]) => area);


    const top = ranked[0];
    const descs = {
        'TI':               'Você tem perfil analítico e tecnológico. Ambientes de inovação e resolução de problemas combinam com você.',
        'Saúde':            'Você tem perfil cuidador e empático. Sua vocação aponta para ajudar e cuidar de pessoas.',
        'Administração':    'Você tem perfil organizacional e estratégico. Liderança e gestão de processos são seus pontos fortes.',
        'Finanças':         'Você tem perfil analítico e metódico. Números, planejamento e controle são sua praia.',
        'Comunicação':      'Você tem perfil expressivo e criativo. Comunicar, persuadir e criar narrativas são seus talentos.',
        'Design':           'Você tem perfil visual e criativo. Estética, inovação e experiência do usuário te movem.',
        'Engenharia':       'Você tem perfil técnico e construtivo. Projetos, estruturas e soluções concretas são sua paixão.',
        'Educação':         'Você tem perfil educador e colaborativo. Transmitir conhecimento e desenvolver pessoas é sua vocação.',
        'Logística':        'Você tem perfil operacional e estratégico. Organizar fluxos e garantir eficiência é sua força.',
        'Indústria':        'Você tem perfil técnico e prático. Trabalhar com processos produtivos e materiais é o que te motiva.',
        'Análise de Dados': 'Você tem perfil investigativo e lógico. Padrões, dados e insights são o que mais te fascinam.',
        'Vendas':           'Você tem perfil comunicativo e orientado a resultados. Negociar e conquistar clientes é natural para você.',
        'Direito':          'Você tem perfil argumentativo e analítico. Regras, justiça e defesa de causas são sua essência.',
    };

    document.getElementById('vocResultDesc').textContent = descs[top] || 'Suas respostas revelam um perfil versátil com múltiplas afinidades.';


    const areasContainer = document.getElementById('vocAreas');
    areasContainer.innerHTML = '';
    ranked.forEach((areaKey, idx) => {
        const area = VOC_AREAS[areaKey];
        if (!area) return;
        const card = document.createElement('div');
        card.className = 'voc-area-card';
        card.setAttribute('role', 'listitem');
        if (idx === 0) card.classList.add('voc-area-card--top');
        card.innerHTML = `
            <div class="voc-area-card__header">
                <span class="voc-area-card__icon" aria-hidden="true">${area.icon}</span>
                <div>
                    <strong>${area.label}</strong>
                    ${idx === 0 ? '<span class="me-badge me-badge--ok">Melhor match</span>' : ''}
                </div>
            </div>
            <p class="voc-area-card__vagas-label">Vagas relacionadas:</p>
            <ul class="voc-area-card__vagas" aria-label="Vagas em ${area.label}">
                ${area.vagas.map(v => `<li>${v}</li>`).join('')}
            </ul>
        `;
        areasContainer.appendChild(card);
    });


    try {
        const raw = localStorage.getItem('od-session');
        if (raw) {
            const session = JSON.parse(raw);
            session.vocacional = { areas: ranked, done: true };
            localStorage.setItem('od-session', JSON.stringify(session));
        }
    } catch(e) {}


    document.getElementById('voc-test').style.display   = 'none';
    document.getElementById('voc-result').style.display = 'block';


    const badge = document.getElementById('voc-badge');
    badge.textContent = 'Concluído ✓';
    badge.className   = 'me-badge me-badge--ok';

    document.getElementById('voc-result').scrollIntoView({ behavior: 'smooth', block: 'start' });

    vocDone = true;
}

function vocRestart() {
    document.getElementById('voc-result').style.display = 'none';
    document.getElementById('voc-test').style.display   = 'block';
    vocCurrent = 0;
    vocAnswers = {};
    vocRenderQuestion();
}


document.addEventListener('DOMContentLoaded', () => {
    try {
        const raw = localStorage.getItem('od-session');
        if (!raw) return;
        const session = JSON.parse(raw);
        if (session.vocacional?.done) {

            const badge = document.getElementById('voc-badge');
            badge.textContent = 'Concluído ✓';
            badge.className   = 'me-badge me-badge--ok';

            const introBtn = document.querySelector('#voc-intro .me-btn--primary');
            if (introBtn) introBtn.textContent = 'Refazer teste';
        }
    } catch(e) {}
});

// FIM SEÇÃO TESTE VOCACIONAL
