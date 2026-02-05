import { useState, useEffect } from 'react';

export const useSettings = () => {
    const [settings, setSettings] = useState({
        defaultGroup: '',
        showAll: false,
        lectures: false,
        seminars: true,
        labs: true
    });
    const [isLoading, setIsLoading] = useState(true);

    // Загрузка настроек из cookies
    useEffect(() => {
        const savedSettings = getSettingsFromCookies();
        if (savedSettings) {
            setSettings(savedSettings);
        }
        setIsLoading(false);
    }, []);

    // Сохранение настроек в cookies при изменении
    useEffect(() => {
        if (!isLoading) {
            saveSettingsToCookies(settings);
        }
    }, [settings, isLoading]);

    const updateSettings = (newSettings) => {
        setSettings(prev => ({ ...prev, ...newSettings }));
    };

    return { settings, updateSettings, isLoading };
};

// Функции для работы с cookies остаются без изменений
const getSettingsFromCookies = () => {
    try {
        const cookies = document.cookie.split(';');
        const settingsCookie = cookies.find(cookie => cookie.trim().startsWith('appSettings='));

        if (settingsCookie) {
            const settingsJson = settingsCookie.split('=')[1];
            return JSON.parse(decodeURIComponent(settingsJson));
        }
    } catch (error) {
        console.error('Error parsing settings from cookies:', error);
    }
    return null;
};

const saveSettingsToCookies = (settings) => {
    try {
        const settingsJson = JSON.stringify(settings);
        const expires = new Date();
        expires.setFullYear(expires.getFullYear() + 1);

        document.cookie = `appSettings=${encodeURIComponent(settingsJson)}; expires=${expires.toUTCString()}; path=/; SameSite=Lax`;
    } catch (error) {
        console.error('Error saving settings to cookies:', error);
    }
};