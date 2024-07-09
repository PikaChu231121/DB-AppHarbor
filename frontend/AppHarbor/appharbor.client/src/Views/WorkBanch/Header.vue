<!--标题栏组件实现-->
<template>
    <header class="header">
        <div class="logo-container">
            <img loading="lazy" src="@/../public/logo.png" class="logo-image" alt="Semi Templates Logo" />
            <img loading="lazy" src="@/../public/logo-text.png" class="logo-text-image" alt="AppHabour Logo Text" />
        </div>
    </header>
</template>

<style scoped>
    .header {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        display: flex;
        justify-content: space-between;
        padding: 12px 0;
        gap: 20px;
        background-color: #efc2bb; /* Optional: Add a background color */
        z-index: 1000; /* Ensure it stays on top of other elements */
    }

    .logo-container {
        display: flex;
        gap: 8px;
        align-items: center;
        margin-left: 10px; /* Move logo-container to the right */
    }

    .logo-image {
        width: 36px;
        height: 36px;
    }

    .logo-text-image {
        height: 36px; /* Adjust height as needed */
    }


    @media (max-width: 991px) {
        .header {
            flex-wrap: wrap;
        }
    }
</style>