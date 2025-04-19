// Funções de máscara
function applyMasks() {
    //máscara para CNPJ (00.000.000/0000-00)
    const cnpjField = document.getElementById('cnpj');
    if (cnpjField) {
        cnpjField.addEventListener('input', function (e) {
            let value = e.target.value.replace(/\D/g, '');
            if (value.length > 12) {
                value = value.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, '$1.$2.$3/$4-$5');
            } else if (value.length > 8) {
                value = value.replace(/^(\d{2})(\d{3})(\d{3})/, '$1.$2.$3');
            } else if (value.length > 5) {
                value = value.replace(/^(\d{2})(\d{3})/, '$1.$2');
            }
            e.target.value = value.substring(0, 18);
        });
    }

    //máscara para Telefone ((00) 00000-0000)
    const phoneField = document.getElementById('phone');
    if (phoneField) {
        phoneField.addEventListener('input', function (e) {
            let value = e.target.value.replace(/\D/g, '');
            if (value.length > 6) {
                value = value.replace(/^(\d{2})(\d{5})(\d{4})/, '($1) $2-$3');
            } else if (value.length > 2) {
                value = value.replace(/^(\d{2})/, '($1) ');
            }
            e.target.value = value.substring(0, 15);
        });
    }

    //máscara para CEP (00000-000)
    const cepField = document.getElementById('cep');
    if (cepField) {
        cepField.addEventListener('input', function (e) {
            let value = e.target.value.replace(/\D/g, '');
            if (value.length > 5) {
                value = value.replace(/^(\d{5})(\d{3})/, '$1-$2');
            }
            e.target.value = value.substring(0, 9);
        });
    }
}

// Exporta a função para ser chamada pelo Blazor
window.applyMasks = applyMasks;

// Funções de máscara para Doador
// Funções de máscara - Versão Estável Sem Debug
function applyDoadorMasks() {
    // Verifica se é pessoa física de forma segura
    function isPessoaFisica() {
        const radio = document.querySelector('input[name="TipoPessoa"][value="true"]');
        return radio ? radio.checked : true;
    }

    // Formata CPF (000.000.000-00)
    function formatCPF(value) {
        const digits = (value || '').replace(/\D/g, '').substr(0, 11);
        if (digits.length <= 3) return digits;
        if (digits.length <= 6) return digits.replace(/(\d{3})(\d{0,3})/, '$1.$2');
        if (digits.length <= 9) return digits.replace(/(\d{3})(\d{3})(\d{0,3})/, '$1.$2.$3');
        return digits.replace(/(\d{3})(\d{3})(\d{3})(\d{0,2})/, '$1.$2.$3-$4');
    }

    // Formata CNPJ (00.000.000/0000-00)
    function formatCNPJ(value) {
        const digits = (value || '').replace(/\D/g, '').substr(0, 14);
        if (digits.length <= 2) return digits;
        if (digits.length <= 5) return digits.replace(/(\d{2})(\d{0,3})/, '$1.$2');
        if (digits.length <= 8) return digits.replace(/(\d{2})(\d{3})(\d{0,3})/, '$1.$2.$3');
        if (digits.length <= 12) return digits.replace(/(\d{2})(\d{3})(\d{3})(\d{0,4})/, '$1.$2.$3/$4');
        return digits.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{0,2})/, '$1.$2.$3/$4-$5');
    }

    // Configura campo CPF/CNPJ
    function setupCPFCNPJ() {
        const field = document.getElementById('cpf-cnpj');
        if (!field) return;

        const pessoaFisica = isPessoaFisica();
        field.placeholder = pessoaFisica ? '000.000.000-00' : '00.000.000/0000-00';
        field.maxLength = pessoaFisica ? 14 : 18;

        field.addEventListener('input', function (e) {
            const start = e.target.selectionStart;
            e.target.value = pessoaFisica ? formatCPF(e.target.value) : formatCNPJ(e.target.value);
            e.target.setSelectionRange(start, start);
        });
    }

    // Configura máscara de telefone
    function setupTelefone() {
        const field = document.getElementById('phone');
        if (!field) return;

        field.addEventListener('input', function (e) {
            const start = e.target.selectionStart;
            const digits = e.target.value.replace(/\D/g, '').substr(0, 11);

            let formatted = digits;
            if (digits.length > 2) formatted = `(${digits.substr(0, 2)}) ${digits.substr(2)}`;
            if (digits.length > 7) formatted = formatted.replace(/(\(\d{2}\) \d{5})(\d{0,4})/, '$1-$2');

            e.target.value = formatted;
            e.target.setSelectionRange(start, start);
        });
    }

    // Configura máscara de CEP
    function setupCEP() {
        const field = document.getElementById('cep');
        if (!field) return;

        field.addEventListener('input', function (e) {
            const start = e.target.selectionStart;
            const digits = e.target.value.replace(/\D/g, '').substr(0, 8);
            e.target.value = digits.length > 5 ? `${digits.substr(0, 5)}-${digits.substr(5)}` : digits;
            e.target.setSelectionRange(start, start);
        });
    }

    // Inicializa todas as máscaras
    function initMasks() {
        setupCPFCNPJ();
        setupTelefone();
        setupCEP();

        // Atualiza máscara quando mudar tipo de pessoa
        document.querySelectorAll('input[name="TipoPessoa"]').forEach(radio => {
            radio.addEventListener('change', function () {
                const field = document.getElementById('cpf-cnpj');
                if (field) {
                    field.value = '';
                    setupCPFCNPJ();
                }
            });
        });
    }

    // Aguarda DOM estar pronto
    if (document.readyState === 'complete') {
        initMasks();
    } else {
        document.addEventListener('DOMContentLoaded', initMasks);
    }
}

// Exporta de forma segura
if (typeof window !== 'undefined') {
    window.applyDoadorMasks = applyDoadorMasks;
}