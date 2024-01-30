function openLightbox() {
    var imageUrl = document.querySelector('.post-image').src;
    document.getElementById('lightbox-image').src = imageUrl;
    document.getElementById('lightbox-modal').classList.add('open');
}

function closeLightbox() {
    document.getElementById('lightbox-modal').classList.remove('open');
}

function submitForm() {
    document.getElementById('myForm').submit();
}