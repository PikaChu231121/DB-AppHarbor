<template>
    <header class="header">
        <div class="logo-container">
            <img loading="lazy" src="https://cdn.builder.io/api/v1/image/assets/TEMP/f3216bdf59b661894810f5395dd3d71cd672dd691475b6cabc44b078e13b5787?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&" class="logo-image" alt="Semi Templates Logo" />
            <h1 class="logo-text">Semi Templates</h1>
        </div>
        <nav class="navigation">
            <a href="#" class="nav-link">Home</a>
            <a href="#" class="nav-link">Project</a>
            <a href="#" class="nav-link">Board</a>
            <a href="#" class="nav-link active">Forms</a>
        </nav>
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
        background-color: white; /* Optional: Add a background color */
        z-index: 1000; /* Ensure it stays on top of other elements */
    }

    .logo-container {
        display: flex;
        gap: 8px;
        align-items: center;
    }

    .logo-image {
        width: 36px;
        height: 36px;
    }

    .logo-text {
        font-family: Inter, sans-serif;
        font-size: 18px;
        font-weight: 700;
        color: #1c1f23;
        letter-spacing: -0.54px;
        line-height: 1.33;
        margin: 0;
    }

    .navigation {
        display: flex;
        gap: 20px;
        padding: 8px 32px 8px 0;

    }

    .nav-link {
        font-family: Inter, sans-serif;
        font-size: 14px;
        font-weight: 600;
        color: rgba(28, 31, 35, 0.6);
        text-decoration: none;
        letter-spacing: -0.14px;
        line-height: 1.43;
    }

        .nav-link.active {
            color: #1c1f23;
        }

    @media (max-width: 991px) {
        .header {
            flex-wrap: wrap;
        }

        .navigation {
            flex-wrap: wrap;
        }
    }
</style>