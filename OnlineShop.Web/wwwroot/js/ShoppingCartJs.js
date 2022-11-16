function ToggleUpdateQuantityButton(id, visible) {
    var updateQuantityButton = document.getElementById(`${id}`);

    visible
        ? updateQuantityButton.style.display = "inline-block"
        : updateQuantityButton.style.display = "none";
}