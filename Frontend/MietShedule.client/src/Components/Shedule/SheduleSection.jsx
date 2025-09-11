import useDate from "../../hooks/useDate";
import DatePicker from "../DatePicker";
import ShedulePair from "./ShedulePair";
import "./SheduleSection.css"
import { useCallback, useEffect, useState } from "react";

export default function SheduleSection() {
    const [group, setGroup] = useState('')
    const [groupsList, setGroupsList] = useState()
    const [shedule, setShedule] = useState()
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
            let query = `Shedule/${group}?dateString=${(new Date(date.value)).toISOString().slice(0, 10)}`
            const sheduleResponse = await fetch(query)
            if (sheduleResponse.status == 200) {
                const sheduleRecords = await sheduleResponse.json()
                setShedule(sheduleRecords)
            }
        }
    }, [group, date.value])

    // получение списка групп при старте
    useEffect(() => {
        fetchGroupsList()
    }, [])

    // получение расписания при изменении ввода
    useEffect(() => {
        if (groupsList != undefined && groupsList.includes(group)) {
            fetchShedule()
            setInvalidGroup(false)
        }
        else {
            setShedule(undefined)
            if (group.length != 0) {
                setInvalidGroup(true)
            }
        }
    }, [group, date.value, groupsList])

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
                            onChange={(event) => setGroup(event.target.value)} />
                        <datalist id="groups">
                            {groupsList.map(g => <option key={g}>{g}</option>)}
                        </datalist>
                    </div>
                    <DatePicker {...date} />
                </>
            }

            {shedule != undefined &&
                shedule.map(c =>
                    <ShedulePair key={keyExtractor(c)} {...c}></ShedulePair>
                )
            }

            {
                shedule != undefined && shedule.length == 0 &&
                <h2>Нет занятий</h2>
            }
        </section>
    )
}