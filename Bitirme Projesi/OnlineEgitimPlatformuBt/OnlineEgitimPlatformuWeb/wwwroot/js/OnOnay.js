// onayModal.js

// Modal HTML yapısını dinamik olarak oluştur
function createConfirmationModal() {
    const modalHTML = `
        <div id="confirmation-modal" class="modal" style="display: none;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <a href="#" data-dismiss="modal" aria-hidden="true" class="close" onclick="closeConfirmationModal()">×</a>
                        <h3>Emin misin</h3>
                    </div>
                    <div class="modal-body">
                        <p>İşlemi yapmak istediğinize emin misiniz?</p>
                    </div>
                    <div class="modal-footer">
                        <a href="#" id="btnYes" class="btn confirm">Evet</a>
                        <a href="#" data-dismiss="modal" aria-hidden="true" class="btn secondary" onclick="closeConfirmationModal()">Hayır</a>
                    </div>
                </div>
            </div>
        </div>
    `;

    document.body.insertAdjacentHTML('beforeend', modalHTML);
}

// Modal penceresini kapat
function closeConfirmationModal() {
    document.getElementById('confirmation-modal').style.display = 'none';
}

// Form gönderiminden önce onay almak için fonksiyon
function onaydanOnceSor(event) {
    event.preventDefault(); // Formun gönderilmesini engeller
    const modal = document.getElementById('confirmation-modal');
    if (!modal) {
        createConfirmationModal();
    }
    document.getElementById('confirmation-modal').style.display = 'block';

    document.getElementById('btnYes').onclick = function () {
        closeConfirmationModal();
        event.target.submit(); // Formu gönder
    };
}

// Sayfa yüklendiğinde tüm formlara onay fonksiyonunu ekle
document.addEventListener('DOMContentLoaded', function () {
    const forms = document.querySelectorAll('form[onsubmit="onaydanOnceSor(event)"]');
    forms.forEach(form => {
        form.addEventListener('submit', onaydanOnceSor);
    });
});
