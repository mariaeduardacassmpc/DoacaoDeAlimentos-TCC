// Funções de máscara
function applyOngMasks() {
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
if (typeof window !== 'undefined') {
    window.applyOngMasks = applyOngMasks;
}

// Funções de máscara para Doador
    // wwwroot/js/masks.js - CÓDIGO COMPLETO E FUNCIONAL

    function applyDoadorMasks() {
        // Função auxiliar segura para selecionar elementos
        function getElement(id) {
            return document.getElementById(id);
        }

        // Verifica se é pessoa física (com fallback seguro)
        function isPessoaFisica() {
            try {
                const radio = getElement('pessoaFisica');
                return radio ? radio.checked : true;
            } catch {
                return true;
            }
        }

        // Formata CPF (000.000.000-00) - 11 dígitos
        function formatCPF(value) {
            if (!value) return '';
            const digits = value.replace(/\D/g, '').substring(0, 11);

            if (digits.length <= 3) return digits;
            if (digits.length <= 6) return digits.replace(/(\d{3})(\d{1,3})/, '$1.$2');
            if (digits.length <= 9) return digits.replace(/(\d{3})(\d{3})(\d{1,3})/, '$1.$2.$3');
            return digits.replace(/(\d{3})(\d{3})(\d{3})(\d{1,2})/, '$1.$2.$3-$4');
        }

        // Formata CNPJ (00.000.000/0000-00) - 14 dígitos
        function formatCNPJ(value) {
            if (!value) return '';
            const digits = value.replace(/\D/g, '').substring(0, 14);

            if (digits.length <= 2) return digits;
            if (digits.length <= 5) return digits.replace(/(\d{2})(\d{1,3})/, '$1.$2');
            if (digits.length <= 8) return digits.replace(/(\d{2})(\d{3})(\d{1,3})/, '$1.$2.$3');
            if (digits.length <= 12) return digits.replace(/(\d{2})(\d{3})(\d{3})(\d{1,4})/, '$1.$2.$3/$4');
            return digits.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{1,2})/, '$1.$2.$3/$4-$5');
        }

        // Formata Telefone ((00) 00000-0000) - 11 dígitos
        function formatTelefone(value) {
            if (!value) return '';
            const digits = value.replace(/\D/g, '').substring(0, 11);

            if (digits.length <= 2) return digits;
            if (digits.length <= 7) return `(${digits.substring(0, 2)}) ${digits.substring(2)}`;
            return `(${digits.substring(0, 2)}) ${digits.substring(2, 7)}-${digits.substring(7)}`;
        }

        // Formata CEP (00000-000) - 8 dígitos
        function formatCEP(value) {
            if (!value) return '';
            const digits = value.replace(/\D/g, '').substring(0, 8);
            return digits.length > 5 ? `${digits.substring(0, 5)}-${digits.substring(5)}` : digits;
        }

        // Configura máscara CPF/CNPJ
        function setupCPFCNPJ() {
            const field = getElement('cpf-cnpj');
            if (!field) return;

            const pessoaFisica = isPessoaFisica();

            // Configuração inicial
            field.placeholder = pessoaFisica ? '000.000.000-00' : '00.000.000/0000-00';
            field.maxLength = pessoaFisica ? 14 : 18;
            field.value = pessoaFisica ? formatCPF(field.value) : formatCNPJ(field.value);

            // Evento de digitação
            field.addEventListener('input', function (e) {
                const startPos = e.target.selectionStart;
                const pessoaFisica = isPessoaFisica();

                e.target.value = pessoaFisica ? formatCPF(e.target.value) : formatCNPJ(e.target.value);
                e.target.maxLength = pessoaFisica ? 14 : 18;

                // Mantém posição do cursor
                if (startPos !== null) {
                    const newPos = Math.min(startPos, e.target.value.length);
                    e.target.setSelectionRange(newPos, newPos);
                }
            });
        }

        // Configura máscara de Telefone
        function setupTelefone() {
            const field = getElement('phone');
            if (!field) return;

            field.maxLength = 15;
            field.value = formatTelefone(field.value);

            field.addEventListener('input', function (e) {
                const startPos = e.target.selectionStart;
                e.target.value = formatTelefone(e.target.value);

                if (startPos !== null) {
                    const newPos = Math.min(startPos, e.target.value.length);
                    e.target.setSelectionRange(newPos, newPos);
                }
            });
        }

        // Configura máscara de CEP
        function setupCEP() {
            const field = getElement('cep');
            if (!field) return;

            field.maxLength = 9;
            field.value = formatCEP(field.value);

            field.addEventListener('input', function (e) {
                const startPos = e.target.selectionStart;
                e.target.value = formatCEP(e.target.value);

                if (startPos !== null) {
                    const newPos = Math.min(startPos, e.target.value.length);
                    e.target.setSelectionRange(newPos, newPos);
                }
            });
        }

        // Configura troca entre CPF/CNPJ
        function setupTipoPessoaChange() {
            document.querySelectorAll('input[name="TipoPessoa"]').forEach(radio => {
                radio.addEventListener('change', function () {
                    const cpfCnpjField = getElement('cpf-cnpj');
                    if (cpfCnpjField) {
                        cpfCnpjField.value = '';
                        const pessoaFisica = isPessoaFisica();
                        cpfCnpjField.placeholder = pessoaFisica ? '000.000.000-00' : '00.000.000/0000-00';
                        cpfCnpjField.maxLength = pessoaFisica ? 14 : 18;
                    }
                });
            });
        }

        // Inicialização completa
        function initializeMasks() {
            setupCPFCNPJ();
            setupTelefone();
            setupCEP();
            setupTipoPessoaChange();

            console.log('Máscaras configuradas com sucesso!');
        }

        // Aguarda DOM estar pronto
        if (document.readyState === 'complete') {
            initializeMasks();
        } else {
            document.addEventListener('DOMContentLoaded', initializeMasks);
        }
    }

    // Exporta função global com verificação de segurança
    if (typeof window !== 'undefined') {
        window.applyDoadorMasks = applyDoadorMasks;
    }