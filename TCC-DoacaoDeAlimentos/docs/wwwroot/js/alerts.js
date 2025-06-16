window.Alertas = {
   mostrarAlerta: function (mensagem) {
        Swal.fire({
            icon: 'warning',
            title: 'Atenção',
            text: mensagem,
            confirmButtonText: 'OK',
            confirmButtonColor: '#198754'

        });
   }
}
