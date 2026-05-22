/* Lógica exclusiva da página AcessoEmpresas */


// BUSCA DE CEP
async function fetchCEP() {
  const cep = document.getElementById('co-cep').value.replace(/\D/g, '');
  if (cep.length !== 8) return;

  const cityInput   = document.getElementById('co-city');
  const spinner     = document.getElementById('cep-spinner-co');

  cityInput.value   = 'Buscando...';
  if (spinner) spinner.style.display = 'block';

  try {
    const res  = await fetch(`https://viacep.com.br/ws/${cep}/json/`);
    const data = await res.json();

    if (!data.erro) {
      cityInput.value = `${data.localidade} — ${data.uf}`;
      cityInput.setAttribute('readonly', '');
    } else {
      cityInput.value = '';
      cityInput.placeholder = 'Preencha manualmente';
      cityInput.removeAttribute('readonly'); // libera edição manual
      showError('co-cep', 'CEP não encontrado.');
    }
  } catch (e) {
    cityInput.value = '';
    cityInput.placeholder = 'Preencha manualmente';
    cityInput.removeAttribute('readonly'); // sem internet? usuário preenche
    console.warn('Erro ao buscar CEP:', e);
    showError('co-cep', 'Não foi possível buscar o CEP. Preencha a cidade manualmente.');
  } finally {
    if (spinner) spinner.style.display = 'none';
  }
}


// CADASTRO DE EMPRESA
function companyRegister() {
  const name    = document.getElementById('co-name').value.trim();
  const contact = document.getElementById('co-contact').value.trim();
  const role    = document.getElementById('co-role').value.trim();
  const email   = document.getElementById('co-email').value.trim();
  const phone   = document.getElementById('co-phone').value.trim();
  const cep     = document.getElementById('co-cep').value.trim();
  const city    = document.getElementById('co-city').value.trim();
  const terms   = document.getElementById('co-terms').checked;

  clearErrors();

  if (!name)    return showError('co-name',    'Campo obrigatório.');
  if (!contact) return showError('co-contact', 'Campo obrigatório.');
  if (!role)    return showError('co-role',    'Campo obrigatório.');
  if (!email)   return showError('co-email',   'Campo obrigatório.');
  if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email))
                return showError('co-email',   'E-mail inválido.');
  if (!phone)   return showError('co-phone',   'Campo obrigatório.');
  if (cep.replace(/\D/g, '').length < 8)
                return showError('co-cep',     'CEP inválido.');
  if (!terms)   return showError('co-terms-wrap', 'Aceite os termos para continuar.');

  
  const btn = document.querySelector('#screen-register .btn-primary');
  if (btn) {
    btn.disabled = true;
    btn.textContent = 'Enviando...';
    btn.setAttribute('aria-busy', 'true');
  }

  // Simula envio (SUBSTITUIR ISSO POR CHAMADA REAL À API!!!!!!!!!)
  setTimeout(() => {
    document.getElementById('s-company').textContent = name;
    document.getElementById('s-contact').textContent = contact;
    document.getElementById('s-role').textContent    = role;
    document.getElementById('s-email').textContent   = email;
    document.getElementById('s-city').textContent    = city || cep;

    showScreen('screen-success');

    if (btn) {
      btn.disabled = false;
      btn.textContent = 'Enviar cadastro';
      btn.removeAttribute('aria-busy');
    }
  }, 1000);
}
