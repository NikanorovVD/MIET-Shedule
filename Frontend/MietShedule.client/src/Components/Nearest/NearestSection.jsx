import "./NearestSection.css"
import { useCallback, useEffect, useState } from "react";
import NearestPair from "./NearestPair";

export default function NearestSection({ defaultGroup }) {
    const [group, setGroup] = useState(defaultGroup || "")
    const [groupsList, setGroupsList] = useState()
    const [shedule, setShedule] = useState()
    const [loading, setLoading] = useState(false)
    const [invalidGroup, setInvalidGroup] = useState(false)
    const [filters, setFilters] = useState({
        showAll: false,
        showLectures: false,
        showPracticals: true,
        showLabs: true
    })

    // запрос списка групп
    const fetchGroupsList = useCallback(async () => {
        const groupsListResponse = await fetch("Groups")
        if (groupsListResponse.status == 200) {
            const groups = await groupsListResponse.json()
            setGroupsList(groups)
        }
    }, [])

    // Проверка, выбран ли хотя бы один фильтр
    const hasActiveFilters = useCallback(() => {
        return filters.showAll || filters.showLectures || filters.showPracticals || filters.showLabs;
    }, [filters])

    // Генерация filerString на основе выбранных фильтров
    const generateFilerString = useCallback(() => {
        if (filters.showAll) {
            return ".*";
        }

        const types = [];
        if (filters.showLectures) types.push("лек");
        if (filters.showPracticals) types.push("пр");
        if (filters.showLabs) types.push("лаб");

        if (types.length === 0) return null;

        return `\\[(${types.join('|')})\\]`;
    }, [filters])

    // запрос расписания группы
    const fetchShedule = useCallback(async () => {
        if (group && group.length != 0) {
            const filerString = generateFilerString();

            // Если не выбран ни один фильтр - очищаем расписание и не делаем запрос
            if (filerString === null) {
                setShedule(undefined);
                return;
            }

            setLoading(true)
            try {
                let query = `Shedule/nearest/${group}?filerString=${encodeURIComponent(filerString)}`
                const sheduleResponse = await fetch(query)
                if (sheduleResponse.status == 200) {
                    const sheduleRecords = await sheduleResponse.json()
                    setShedule(sheduleRecords)
                }
            } finally {
                setLoading(false)
            }
        }
    }, [group, generateFilerString])

    // получение списка групп при старте
    useEffect(() => {
        fetchGroupsList()
    }, [])

    useEffect(() => {
        if (groupsList === undefined) return;

        const currentGroup = group || "";

        // Если группа не введена
        if (!currentGroup || currentGroup.length === 0) {
            setShedule(undefined);
            setInvalidGroup(false);
            return;
        }

        // Если группа не существует в списке
        if (!groupsList.includes(currentGroup)) {
            setShedule(undefined);
            setInvalidGroup(true);
            return;
        }

        // Группа валидна
        if (hasActiveFilters()) {
            fetchShedule();
            setInvalidGroup(false);
        } else {
            // Группа валидна, но нет активных фильтров
            setShedule(undefined);
            setInvalidGroup(false);
        }
    }, [group, groupsList, defaultGroup, fetchShedule, hasActiveFilters]);

    // Обработчик изменения чекбоксов
    const handleFilterChange = (filterName) => {
        if (filterName === 'showAll') {
            setFilters({
                showAll: true,
                showLectures: false,
                showPracticals: false,
                showLabs: false
            })
        } else {
            setFilters(prev => ({
                ...prev,
                showAll: false,
                [filterName]: !prev[filterName]
            }))
        }
    }

    function keyExtractor(couple) {
        return `${couple.name}-${couple.teacherName}-${couple.auditorium}-${couple.remainingDays}`;
    }

    return (
        <section className="nearest-section">
            {groupsList != undefined &&
                <>
                    <div className="input-container">
                        <input
                            className={`shedule-form-in ${invalidGroup ? "error-input" : ""}`}
                            type="text"
                            list="groups"
                            value={group}
                            placeholder="Учебная группа"
                            onChange={(event) => setGroup(event.target.value)} />
                        <datalist id="groups">
                            {groupsList.map(g => <option key={g}>{g}</option>)}
                        </datalist>
                    </div>

                    <div className="filters-container">
                        <label className="filter-checkbox">
                            <input
                                type="checkbox"
                                checked={filters.showAll}
                                onChange={() => handleFilterChange('showAll')}
                            />
                            <div className="checkbox-custom"></div>
                            <span>Все</span>
                        </label>

                        <label className="filter-checkbox">
                            <input
                                type="checkbox"
                                checked={filters.showLectures}
                                onChange={() => handleFilterChange('showLectures')}
                            />
                            <div className="checkbox-custom"></div>
                            <span>Лекции</span>
                        </label>

                        <label className="filter-checkbox">
                            <input
                                type="checkbox"
                                checked={filters.showPracticals}
                                onChange={() => handleFilterChange('showPracticals')}
                            />
                            <div className="checkbox-custom"></div>
                            <span>Практики</span>
                        </label>

                        <label className="filter-checkbox">
                            <input
                                type="checkbox"
                                checked={filters.showLabs}
                                onChange={() => handleFilterChange('showLabs')}
                            />
                            <div className="checkbox-custom"></div>
                            <span>Лабораторные</span>
                        </label>
                    </div>
                </>
            }

            {loading && (
                <div className="spinner">
                    <div className="spinner-circle"></div>
                </div>
            )}

            {!loading && shedule != undefined &&
                shedule.map(c =>
                    <NearestPair key={keyExtractor(c)} {...c}></NearestPair>
                )
            }

            {
                !loading && shedule != undefined && shedule.length == 0 &&
                <h2>Нет занятий</h2>
            }
        </section>
    )
}