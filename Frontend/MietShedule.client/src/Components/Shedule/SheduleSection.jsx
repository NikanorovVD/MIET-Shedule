import useDate from "../../hooks/useDate";
import DatePicker from "../DatePicker";
import ShedulePair from "./ShedulePair";
import "./SheduleSection.css"
import { useCallback, useEffect, useState } from "react";

export default function SheduleSection({ defaultGroup }) {
    const [group, setGroup] = useState(defaultGroup)
    const [groupsList, setGroupsList] = useState()
    const [shedule, setShedule] = useState()
    const [loading, setLoading] = useState(false)
    const date = useDate()
    const [invalidGroup, setInvalidGroup] = useState(false)

    // запрос списка групп
    const fetchGroupsList = useCallback(async () => {
        const groupsListResponse = await fetch("Groups")
        if (groupsListResponse.status == 200) {
            const groups = await groupsListResponse.json()
            setGroupsList(groups)
        }
    }, [])

    // запрос расписания группы
    const fetchShedule = useCallback(async () => {
        if (group.length != 0) {
            setLoading(true)
            try {
                let query = `Shedule/${group}?dateString=${(new Date(date.value)).toISOString().slice(0, 10)}`
                const sheduleResponse = await fetch(query)
                if (sheduleResponse.status == 200) {
                    const sheduleRecords = await sheduleResponse.json()
                    setShedule(sheduleRecords)
                }
            } finally {
                setLoading(false)
            }
        }
    }, [group, date.value])

    // получение списка групп при старте
    useEffect(() => {
        fetchGroupsList()
    }, [])

    useEffect(() => {
        if (groupsList === undefined) return;

        const currentGroup = group || defaultGroup;

        if (currentGroup && currentGroup.length !== 0 && groupsList.includes(currentGroup)) {
            fetchShedule();
            setInvalidGroup(false);
        } else {
            setShedule(undefined);
            setInvalidGroup(!!(currentGroup && currentGroup.length !== 0));
        }
    }, [group, date.value, groupsList, defaultGroup]);

    function keyExtractor(couple) {
        return `${couple.name}-${couple.teacher}-${couple.order}-${couple.auditorium}-${couple.group}`;
    }

    return (
        <section className="shedule_section">
            {groupsList != undefined &&
                <>
                    <div>
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
                    <DatePicker {...date} />
                </>
            }

            {loading && (
                <div className="spinner">
                    <div className="spinner-circle"></div>
                </div>
            )}

            {!loading && shedule != undefined &&
                shedule.map(c =>
                    <ShedulePair key={keyExtractor(c)} {...c}></ShedulePair>
                )
            }

            {
                !loading && shedule != undefined && shedule.length == 0 &&
                <h2>Нет занятий</h2>
            }
        </section>
    )
}