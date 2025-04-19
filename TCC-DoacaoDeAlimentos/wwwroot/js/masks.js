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

//funções de máscara para Doador
function applyDoadorMasks() {
    //máscara da condição para CPF/CNPJ
    const cpfCnpjField = document.getElementById('cpf-cnpj');
    if (cpfCnpjField) {
        cpfCnpjField.addEventListener('input', function (e) {
            const isPessoaFisica = document.querySelector('input[name="TipoPessoa"]:checked').value === 'true';
            let value = e.target.value.replace(/\D/g, '');

            if (isPessoaFisica) {
                //máscara para CPF (000.000.000-00)
                if (value.length > 9) {
                    value = value.replace(/^(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
                } else if (value.length > 6) {
                    value = value.replace(/^(\d{3})(\d{3})(\d{3})/, '$1.$2.$3');
                } else if (value.length > 3) {
                    value = value.replace(/^(\d{3})(\d{3})/, '$1.$2');
                }
                e.target.value = value.substring(0, 14);
            } else {
                //máscara para CNPJ (00.000.000/0000-00)
                if (value.length > 12) {
                    value = value.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, '$1.$2.$3/$4-$5');
                } else if (value.length > 8) {
                    value = value.replace(/^(\d{2})(\d{3})(\d{3})/, '$1.$2.$3');
                }
                e.target.value = value.substring(0, 18);
            }
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

    //att máscara quando o tipo de pessoa mudar
    const tipoPessoaRadios = document.querySelectorAll('input[name="TipoPessoa"]');
    tipoPessoaRadios.forEach(radio => {
        radio.addEventListener('change', function () {
            const cpfCnpjField = document.getElementById('cpf-cnpj');
            if (cpfCnpjField) {
                cpfCnpjField.value = '';
                cpfCnpjField.dispatchEvent(new Event('input'));
            }
        });
    });
}

window.applyDoadorMasks = applyDoadorMasks;
