import { useState, useCallback, useEffect } from 'react';
import { useSettings } from '../../hooks/useSettings';
import './SettingsSection.css';

export default function SettingsSection() {
    const { settings, updateSettings } = useSettings();
    const [tempGroup, setTempGroup] = useState(settings.defaultGroup || '');
    const [groupsList, setGroupsList] = useState();
    const [invalidGroup, setInvalidGroup] = useState(false);
    const [notification, setNotification] = useState({ show: false, message: '', type: '' });

    // Показ уведомления
    const showNotification = (message, type = 'success') => {
        setNotification({ show: true, message, type });
        setTimeout(() => setNotification({ show: false, message: '', type: '' }), 3000);
    };

    // Синхронизация tempGroup с settings.defaultGroup при загрузке
    useEffect(() => {
        setTempGroup(settings.defaultGroup || '');
    }, [settings.defaultGroup]);

    // запрос списка групп
    const fetchGroupsList = useCallback(async () => {
        const groupsListResponse = await fetch("Groups")
        if (groupsListResponse.status == 200) {
            const groups = await groupsListResponse.json()
            setGroupsList(groups)
        }
    }, [])

    // получение списка групп при старте
    useEffect(() => {
        fetchGroupsList()
    }, [])

    // Проверка валидности группы при изменении
    useEffect(() => {
        if (groupsList === undefined) return;

        if (tempGroup && tempGroup.length !== 0) {
            setInvalidGroup(!groupsList.includes(tempGroup));
        } else {
            setInvalidGroup(false);
        }
    }, [tempGroup, groupsList]);

    const handleSaveAll = () => {
        // Проверяем валидность группы перед сохранением
        if (tempGroup && tempGroup.length !== 0 && groupsList && !groupsList.includes(tempGroup)) {
            showNotification('Ошибка: указана несуществующая группа', 'error');
            return;
        }

        updateSettings({
            defaultGroup: tempGroup,
            showAll: settings.showAll,
            lectures: settings.lectures,
            seminars: settings.seminars,
            labs: settings.labs
        });
        showNotification('Настройки успешно сохранены');
    };

    const handleFilterChange = (filterName) => {
        if (filterName === 'showAll') {
            updateSettings({
                showAll: true,
                lectures: false,
                seminars: false,
                labs: false
            });
        } else {
            updateSettings({
                showAll: false,
                [filterName]: !settings[filterName]
            });
        }
    };

    const handleReset = () => {
        const defaultSettings = {
            defaultGroup: '',
            showAll: false,
            lectures: false,
            seminars: true,
            labs: true
        };
        updateSettings(defaultSettings);
        setTempGroup('');
        setInvalidGroup(false);
        showNotification('Настройки сброшены к значениям по умолчанию');
    };

    return (
        <section className="settings-section">
            {notification.show && (
                <div className={`notification ${notification.type}`}>
                    {notification.message}
                    <button
                        className="notification-close"
                        onClick={() => setNotification({ show: false, message: '', type: '' })}
                    >
                        ×
                    </button>
                </div>
            )}

            <h2>Настройки приложения</h2>

            <div className="settings-group">
                <h3>Группа по умолчанию</h3>
                <div className="input-group">
                    <input
                        className={`settings-input ${invalidGroup ? "error-input" : ""}`}
                        type="text"
                        list="groups"
                        value={tempGroup}
                        placeholder="Учебная группа"
                        onChange={(event) => setTempGroup(event.target.value)}
                    />
                    {groupsList && (
                        <datalist id="groups">
                            {groupsList.map(g => <option key={g}>{g}</option>)}
                        </datalist>
                    )}
                </div>
                {invalidGroup && (
                    <p className="error-text">Указана несуществующая группа</p>
                )}
                <p className="settings-hint">
                    Эта группа будет автоматически подставляться при открытии расписания
                </p>
            </div>

            <div className="settings-group">
                <h3>Типы пар для отображения</h3>
                <div className="filters-container">
                    <label className="filter-checkbox">
                        <input
                            type="checkbox"
                            checked={settings.showAll}
                            onChange={() => handleFilterChange('showAll')}
                        />
                        <div className="checkbox-custom"></div>
                        <span>Все</span>
                    </label>

                    <label className="filter-checkbox">
                        <input
                            type="checkbox"
                            checked={settings.lectures}
                            onChange={() => handleFilterChange('lectures')}
                        />
                        <div className="checkbox-custom"></div>
                        <span>Лекции</span>
                    </label>

                    <label className="filter-checkbox">
                        <input
                            type="checkbox"
                            checked={settings.seminars}
                            onChange={() => handleFilterChange('seminars')}
                        />
                        <div className="checkbox-custom"></div>
                        <span>Семинары</span>
                    </label>

                    <label className="filter-checkbox">
                        <input
                            type="checkbox"
                            checked={settings.labs}
                            onChange={() => handleFilterChange('labs')}
                        />
                        <div className="checkbox-custom"></div>
                        <span>Лабораторные</span>
                    </label>
                </div>
                <p className="settings-hint">
                    Эти настройки влияют на отображение пар в разделе "Ближайшие пары"
                </p>
            </div>

            <div className="settings-actions">
                <button onClick={handleSaveAll} className="save-btn">
                    Сохранить настройки
                </button>
                <button onClick={handleReset} className="reset-btn">
                    Сбросить настройки
                </button>
            </div>
        </section>
    );
}