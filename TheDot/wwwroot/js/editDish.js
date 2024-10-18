function openDishModal(button) {
    const dishId = button.getAttribute('data-dishid');
    const dishName = button.getAttribute('data-dishname');
    const description = button.getAttribute('data-description');
    const price = button.getAttribute('data-price');
    const isAvailable = button.getAttribute('data-isavailable') === 'True';
    const popular = button.getAttribute('data-popular') === 'True';

    document.getElementById('editDishId').value = dishId;
    document.getElementById('editDishName').value = dishName;
    document.getElementById('editDescription').value = description;
    document.getElementById('editPrice').value = price;

    document.getElementById('editIsAvailable').checked = isAvailable;
    document.getElementById('editPopular').checked = popular;

    document.getElementById('editDishModal').classList.remove('hidden');
}

document.getElementById('closeDishModalBtn').addEventListener('click', function () {
    document.getElementById('editDishModal').classList.add('hidden');
});

window.addEventListener('click', function (e) {
    const modal = document.getElementById('editDishModal');
    if (e.target === modal) {
        modal.classList.add('hidden');
    }
});
